#if ADDRESSABLES_PACKAGE_INSTALLED
using System;
using System.IO;
using System.Threading;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using AYellowpaper.SerializedCollections;
using Cysharp.Threading.Tasks;

namespace CSTGames.SharedResources.AddressablesRuntime
{
	public class AddressablesLoader : PersistentSingleton<AddressablesLoader>
	{
		[Header("Labels Dictionary"), Space]
		[SerializeField] private SerializedDictionary<AddressablesLabel, AssetLabelReference> _labelsDictionary;

		public bool IsInitialized => _isInitialized;

		private HashSet<AsyncOperationHandle> _inUsedHandles = new();
		private bool _isInitialized = false;

		private void OnEnable()
		{
			UnsubscribeEvents();
			SubscribeEvents();
		}

		private void OnDisable()
		{
			UnsubscribeEvents();
		}

		private void OnDestroy()
		{
			ReleaseAllHandles();
		}

		#region Loader's Life Cycle Methods
		public void Initialize()
		{
			Debug.Log("<b>Initializing Addressables...</b>");

			Addressables.InitializeAsync(autoReleaseHandle: true).Completed += async (op) =>
			{
				if (op.Status == AsyncOperationStatus.Succeeded)
				{
					Debug.Log("Addressables initialization <b><color=#64AE20>SUCCESS!</color></b>");

					await CheckForCatalogUpdates();
					await CleanUnusedBundleCaches();

					// Pre-download dependencies if needed.
				}
				else
				{
					Debug.LogWarning($"Addressables initialization <b><color=#DB3333>FAILED!</color></b> Catalog requests most likely <b>timed out</b>: {op.DebugName}.\n" +
						$"Reason: {op.OperationException}\n\n" +
						"Attempting to load fallback catalog files...");

					LoadFallbackCatalogs();
				}
			};
		}

		public void LoadFallbackCatalogs()
		{
			string fallbackPath = Path.Combine(Application.streamingAssetsPath, "aa", "catalog.json");

			Addressables.LoadContentCatalogAsync(fallbackPath, autoReleaseHandle: true).Completed += async (op) =>
			{
				if (op.Status == AsyncOperationStatus.Succeeded)
				{
					Debug.Log("Fallback catalogs <b><color=#64AE20>LOADED!</color></b>");

					await CheckForCatalogUpdates();
					await CleanUnusedBundleCaches();

					// Pre-download dependencies if needed.
				}
				else
				{
					Debug.LogError($"Fallback catalogs loading <b><color=#DB3333>FAILED!</color></b> Catalog files might be missing: {op.DebugName}.\n" +
						$"Reason: {op.OperationException}");
				}
			};
		}

		public async UniTask CheckForCatalogUpdates()
		{
			try
			{
				List<string> catalogsToUpdate = await Addressables.CheckForCatalogUpdates(autoReleaseHandle: true);

				if (catalogsToUpdate == null || catalogsToUpdate.Count == 0)
				{
					Debug.Log("No new addressable catalogs available to update.");

					await UniTask.CompletedTask;
					_isInitialized = true;

					return;
				}

				Debug.Log($"<b>Updating {catalogsToUpdate.Count}</b> addressable catalogs...");

				var updateHandle = Addressables.UpdateCatalogs(catalogsToUpdate, autoReleaseHandle: true);
				updateHandle.Completed += (op) => _isInitialized = true;

				await updateHandle.ToUniTask();
			}
			catch (Exception ex)
			{
				Debug.LogError($"Error during checking or updating addressable catalogs: {ex.Message}. Using existing catalogs.");

				await UniTask.CompletedTask;
				_isInitialized = true;
			}
		}

		public async UniTask CleanUnusedBundleCaches()
		{
#if !UNITY_EDITOR
		Debug.Log("<b>Cleaning</b> unused asset bundle caches...");

		await UniTask.Yield();

		var handle = Addressables.CleanBundleCache();
		handle.Completed += (op) =>
		{
			if (op.IsValid())
			{
				Addressables.Release(op);
			}
		};

		try
		{
			await handle.ToUniTask();
		}
		catch (Exception ex)
		{
			Debug.LogError($"Error during cleaning unused bundle caches: {ex.Message}.");
		}
#else
			await UniTask.CompletedTask;
#endif
		}

		public void ReleaseAllHandles()
		{
			Debug.Log("Releasing all addressables handles...");

			ReleaseHandleSet(_inUsedHandles);
		}

		public void ReleaseHandleSet(HashSet<AsyncOperationHandle> handleSet)
		{
			foreach (var handle in handleSet)
			{
				if (handle.IsValid())
				{
					try
					{
						Addressables.Release(handle);
					}
					catch (Exception ex)
					{
						Debug.LogWarning($"[CancelRelease] Exception during Addressables.Release: {ex.Message}");
					}
				}
			}

			handleSet.Clear();
		}

		public void Release<TObject>(TObject loadedObject)
		{
			if (loadedObject == null)
			{
				return;
			}

			try
			{
				_inUsedHandles.RemoveWhere(handle => handle.Result.Equals(loadedObject));

				Addressables.Release(loadedObject);
			}
			catch (Exception ex)
			{
				Debug.LogError($"[CancelRelease] Exception during Addressables.Release: {ex.Message}");
			}
		}

		public void Release(string objectName)
		{
			Debug.Log($"Attempting to release addressable asset with name: {objectName}...");
			if (string.IsNullOrEmpty(objectName))
			{
				return;
			}

			try
			{
				_inUsedHandles.RemoveWhere(handle =>
				{
					UnityEngine.Object loadedObject = handle.Result as UnityEngine.Object;

					if (loadedObject != null && loadedObject.name == objectName)
					{
						Addressables.Release(handle.Result);
						return true;
					}

					return false;
				});

			}
			catch (Exception ex)
			{
				Debug.LogError($"[CancelRelease] Exception during Addressables.Release: {ex.Message}\nStack trace: {ex.StackTrace}");
			}
		}
		#endregion

		#region Asset Loading Methods Using Labels
		public async UniTask LoadAssetAsync<T>
		(
			AddressablesLabel label,
			Action<T> onCompleteCallback,
			Action onFailedCallback = null,
			Action<float> onProgressCallback = null,
			CancellationToken cancellationToken = default
		)
		{
			try
			{
				AsyncOperationHandle<T> handle = default;

				if (_labelsDictionary.TryGetValue(label, out var assetLabelReference))
				{
					handle = Addressables.LoadAssetAsync<T>(assetLabelReference);
					handle.Completed += (op) => OnAssetLoaded(op, onCompleteCallback);
				}

				while (!handle.IsDone)
				{
					onProgressCallback?.Invoke(handle.PercentComplete);
					await UniTask.Yield(PlayerLoopTiming.LastPostLateUpdate, cancellationToken);
				}
			}
			catch (Exception ex)
			{
				Debug.LogError($"<b><color=#DB3333>FAILED</color></b> to load asset with label: {label}. Attempting to fallback...\n\n" +
					$"Reason: {ex.Message}.\n\n" +
					$"Stack trace: {ex.StackTrace}.");

				onFailedCallback?.Invoke();
			}
		}

		public async UniTask LoadAssetsAsync<T>
		(
			AddressablesLabel label,
			Action<T> onEachAssetLoaded,
			Action<IList<T>> onCompleteCallback,
			Action onFailedCallback = null,
			Action<float> onProgressCallback = null,
			CancellationToken cancellationToken = default
		)
		{
			try
			{
				AsyncOperationHandle<IList<T>> handle = default;

				if (_labelsDictionary.TryGetValue(label, out var assetLabelReference))
				{
					handle = Addressables.LoadAssetsAsync(assetLabelReference, onEachAssetLoaded);
					handle.Completed += (op) => OnAssetLoaded(op, onCompleteCallback);
				}

				while (!handle.IsDone)
				{
					onProgressCallback?.Invoke(handle.PercentComplete);
					await UniTask.Yield(PlayerLoopTiming.LastPostLateUpdate, cancellationToken);
				}
			}
			catch (Exception ex)
			{
				Debug.LogError($"<b><color=#DB3333>FAILED</color></b> to load assets with label: {label}. Attempting to fallback...\n\n" +
					$"Reason: {ex.Message}.\n\n" +
					$"Stack trace: {ex.StackTrace}.");

				onFailedCallback?.Invoke();
			}
		}
		#endregion

		#region Asset Loading Methods Using Path
		public async UniTask LoadAssetAsync<T>
		(
			string addressPath,
			Action<T> onCompleteCallback,
			Action onFailedCallback = null,
			Action<float> onProgressCallback = null,
			CancellationToken cancellationToken = default
		)
		{
			try
			{
				var handle = Addressables.LoadAssetAsync<T>(addressPath);
				handle.Completed += (op) => OnAssetLoaded(op, onCompleteCallback);

				while (!handle.IsDone)
				{
					onProgressCallback?.Invoke(handle.PercentComplete);
					await UniTask.Yield(PlayerLoopTiming.LastPostLateUpdate, cancellationToken);
				}
			}
			catch (Exception ex)
			{
				Debug.LogError($"<b><color=#DB3333>FAILED</color></b> to load asset with key: {addressPath}. Attempting to fallback...\n\n" +
					$"Reason: {ex.Message}.\n\n" +
					$"Stack trace: {ex.StackTrace}.");

				onFailedCallback?.Invoke();
			}
		}

		public async UniTask LoadAssetsAsync<T>
		(
			string addressPath,
			Action<T> onEachAssetLoaded,
			Action<IList<T>> onCompleteCallback,
			Action onFailedCallback = null,
			Action<float> onProgressCallback = null,
			CancellationToken cancellationToken = default
		)
		{
			try
			{
				var handle = Addressables.LoadAssetsAsync(addressPath, onEachAssetLoaded);
				handle.Completed += (op) => OnAssetLoaded(op, onCompleteCallback);

				while (!handle.IsDone)
				{
					onProgressCallback?.Invoke(handle.PercentComplete);
					await UniTask.Yield(PlayerLoopTiming.LastPostLateUpdate, cancellationToken);
				}
			}
			catch (Exception ex)
			{
				Debug.LogError($"<b><color=#DB3333>FAILED</color></b> to load assets with key: {addressPath}. Attempting to fallback...\n\n" +
					$"Reason: {ex.Message}.\n\n" +
					$"Stack trace: {ex.StackTrace}.");

				onFailedCallback?.Invoke();
			}
		}
		#endregion

		#region Asset Loading Methods with Multiple Keys.
		public async UniTask LoadAssetsAsync<T>
		(
			IEnumerable<object> keys,
			Addressables.MergeMode mergeMode,
			Action<T> onEachAssetLoaded,
			Action<IList<T>> onCompleteCallback,
			Action onFailedCallback = null,
			Action<float> onProgressCallback = null,
			CancellationToken cancellationToken = default
		)
		{
			try
			{
				var handle = Addressables.LoadAssetsAsync(keys, onEachAssetLoaded, mergeMode);
				handle.Completed += (op) => OnAssetLoaded(op, onCompleteCallback);

				while (!handle.IsDone)
				{
					onProgressCallback?.Invoke(handle.PercentComplete);
					await UniTask.Yield(PlayerLoopTiming.LastPostLateUpdate, cancellationToken);
				}
			}
			catch (Exception ex)
			{
				Debug.LogError($"<b><color=#DB3333>FAILED</color></b> to load assets with keys: {keys}. Attempting to fallback...\n\n" +
					$"Reason: {ex.Message}.\n\n" +
					$"Stack trace: {ex.StackTrace}.");

				onFailedCallback?.Invoke();
			}
		}
		#endregion

		#region Dependencies Download Methods.
		public async void PreDownloadDependencies
		(
			object key,
			Action onCompleteCallback = null,
			Action onFailedCallback = null
		)
		{
			Debug.Log($"<b>Downloading dependencies</b> for \"{key}\"...");

			var handle = Addressables.DownloadDependenciesAsync(key, autoReleaseHandle: true);
			handle.Completed += (op) =>
			{
				if (op.Status == AsyncOperationStatus.Succeeded)
				{
					Debug.Log($"Dependencies for \"{key}\" <b><color=#64AE20>DOWNLOADED!</color></b>");
					onCompleteCallback?.Invoke();
				}
			};

			try
			{
				await handle.ToUniTask();
			}
			catch (Exception ex)
			{
				Debug.LogError($"Dependencies for \"{key}\" <b><color=#DB3333>FAILED</color></b> to be downloaded somehow: {ex.Message}.\n" +
					$"Stack trace: {ex.StackTrace}");

				onFailedCallback?.Invoke();
			}
		}

		public async void CheckResourcesAvailability
		(
			object key,
			Action<IList<IResourceLocation>> onCompleteCallback = null,
			Action onFailedCallback = null
		)
		{
			Debug.Log($"<b>Checking resources availability</b> for \"{key}\"...");

			var handle = Addressables.LoadResourceLocationsAsync(key);
			handle.Completed += (op) =>
			{
				if (op.Result.Count > 0)
				{
					Debug.Log($"Resources for \"{key}\" <b><color=#64AE20>LOCATED!</color></b>");
					onCompleteCallback?.Invoke(op.Result);
					return;
				}

				Debug.LogError($"Resources for \"{key}\" <b><color=#DB3333>NOT FOUND!</color></b> Properly an invalid key.");
				onFailedCallback?.Invoke();
			};

			try
			{
				await handle.ToUniTask();
				Addressables.Release(handle);
			}
			catch (Exception ex)
			{
				Debug.LogError($"Resources for \"{key}\" <b><color=#DB3333>FAILED</color></b> to be located somehow: {ex.Message}.\n" +
					$"Stack trace: {ex.StackTrace}");

				onFailedCallback?.Invoke();
			}
		}
		#endregion

		#region Event Wrappers
		private void SubscribeEvents()
		{

		}

		private void UnsubscribeEvents()
		{

		}

		private void OnAssetLoaded<T>(AsyncOperationHandle<T> op, Action<T> onCompleteCallback)
		{
			if (op.Status != AsyncOperationStatus.Succeeded && op.IsValid())
			{
				Debug.Log("Releasing failed operation handle...");
				Addressables.Release(op);
				return;
			}

			bool isNewHandle = _inUsedHandles.Add(op);
			_inUsedHandles.TryGetValue(op, out var existingHandle);

			if (!isNewHandle && op.IsValid())
			{
				Debug.Log($"Releasing duplicated operation handle of {op.Result}...");
				Addressables.Release(op);

				onCompleteCallback?.Invoke((T)existingHandle.Result);
			}
			else
			{
				onCompleteCallback?.Invoke(op.Result);
			}

			Debug.Log($"In used handles count: {_inUsedHandles.Count}");
		}
		#endregion

		#region Helper Methods
		public async UniTask AsyncWaitForInitialization()
		{
			await UniTask.WaitUntil(() => _isInitialized);
		}
		#endregion
	}

	public enum AddressablesLabel
	{
		Default = 0
	}
}
#endif