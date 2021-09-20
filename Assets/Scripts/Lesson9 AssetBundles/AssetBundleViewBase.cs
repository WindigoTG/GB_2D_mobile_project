using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace AssetBundles
{
    public class AssetBundleViewBase : MonoBehaviour
    {
        private const string ASSET_BUNDLE_SPRITES_URL_LZMA = "https://drive.google.com/uc?export=download&id=1hu0ksWApGdcQ5hSS7JUmk7NA2bK_bG47";
        private const string ASSET_BUNDLE_AUDIO_URL_LZMA = "https://drive.google.com/uc?export=download&id=13oGM58t97EntcVdFZYDb0lz7XVQHXy31";
        private const string ASSET_BUNDLE_SPRITES_URL_LZ4 = "https://drive.google.com/uc?export=download&id=1tiuYXNoBaFSsJy11dYYOC53nMcN35Ho9";
        private const string ASSET_BUNDLE_AUDIO_URL_LZ4 = "https://drive.google.com/uc?export=download&id=19EQea9LfQJJya6HN62cLpKlDXAOSZNWa";
        private const string ASSET_BUNDLE_SPRITES_URL_NONCOMP = "https://drive.google.com/uc?export=download&id=1_EZxI_TluP9D4sedfVXv0GRhHgfdc9si";
        private const string ASSET_BUNDLE_AUDIO_URL_NONCOMP = "https://drive.google.com/uc?export=download&id=1qJex89YNaMdij-h3k6PSw9unqBT3PjxU";

        [SerializeField]
        private DataSpriteBundle[] _dataSpriteBundles;

        [SerializeField]
        private DataAudioBundle[] _dataAudioBundles;

        private AssetBundle _spritesAssetBundle;
        private AssetBundle _audioAssetBundle;

        protected IEnumerator DownloadAndSetAssetBundle()
        {
            yield return GetSpritesAssetBundle();
            yield return GetAudioAssetBundle();

            if (_spritesAssetBundle == null || _audioAssetBundle == null)
            {
                Debug.LogError($"AssetBundle {(_spritesAssetBundle == null ? _spritesAssetBundle : _audioAssetBundle)} failed to load");
                yield break;
            }

            SetDownloadAssets();
            yield return null;
        }

        private IEnumerator GetSpritesAssetBundle()
        {
            var startTime = Time.realtimeSinceStartup;
            var request = UnityWebRequestAssetBundle.GetAssetBundle(ASSET_BUNDLE_SPRITES_URL_LZMA);

            yield return request.SendWebRequest();

            while (!request.isDone)
                yield return null;

            var finishTime = Time.realtimeSinceStartup;
            Debug.Log($"Sprites pack  |  size: {request.downloadedBytes} bytes  |  download time: {finishTime - startTime} sec");

            StateRequest(request, ref _spritesAssetBundle);
        }

        private IEnumerator GetAudioAssetBundle()
        {
            var startTime = Time.realtimeSinceStartup;
            var request = UnityWebRequestAssetBundle.GetAssetBundle(ASSET_BUNDLE_AUDIO_URL_LZMA);

            yield return request.SendWebRequest();

            while (!request.isDone)
                yield return null;

            var finishTime = Time.realtimeSinceStartup;
            Debug.Log($"Audio pack  |  size: {request.downloadedBytes} bytes  |  download time: {finishTime - startTime} sec");

            StateRequest(request, ref _audioAssetBundle);

            yield return null;
        }

        private void StateRequest(UnityWebRequest request, ref AssetBundle assetBundle)
        {
            if (request.error == null)
            {
                assetBundle = DownloadHandlerAssetBundle.GetContent(request);
                Debug.Log("Complete");
            }
            else
            {
                Debug.Log(request.error);
            }
        }

        private void SetDownloadAssets()
        {
            foreach (var data in _dataSpriteBundles)
                data.Image.sprite = _spritesAssetBundle.LoadAsset<Sprite>(data.NameAssetBundle);

            foreach (var data in _dataAudioBundles)
            {
                data.AudioSource.clip = _audioAssetBundle.LoadAsset<AudioClip>(data.NameAssetBundle);
                data.AudioSource.Play();
            }
        }
    }
}