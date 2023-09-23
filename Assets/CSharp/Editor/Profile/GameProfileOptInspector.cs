using UnityEditor;
using UnityEngine;

namespace U3DMobileEditor
{
    [CustomPropertyDrawer(typeof(GameLanguage))]
    internal class GameLanguageDrawer : ListItemDrawer
    {
        protected override void OnDrawLine(int _, SerializedProperty property)
        {
            Label( 60, "Language");
            Field(flx, property, "language");
        }
    }

    [CustomPropertyDrawer(typeof(StoreChannel))]
    internal class StoreChannelDrawer : ListItemDrawer
    {
        protected override void OnDrawLine(int _, SerializedProperty property)
        {
            Label( 60, "Channel");
            Field(flx, property, "channel");
        }
    }

    [CustomPropertyDrawer(typeof(ChannelGateway))]
    internal class ChannelGatewayDrawer : ListItemDrawer
    {
        protected override void OnDrawLine(int _, SerializedProperty property)
        {
            Field( 90, property, "channel");
            Label( 30, "URL");
            Field(flx, property, "gateway");
        }
    }

    [CustomPropertyDrawer(typeof(ForcedURL))]
    internal class ForcedURLDrawer : ListItemDrawer
    {
        protected override void OnDrawLine(int _, SerializedProperty property)
        {
            Field( 90, property, "name");
            Label( 30, "URL");
            Field(flx, property, "url");
        }
    }

    [CustomPropertyDrawer(typeof(AssetFlavor))]
    internal class AssetFlavorDrawer : ListItemDrawer
    {
        protected override void OnDrawLine(int _, SerializedProperty property)
        {
            Label( 60, "Flavor");
            Field(flx, property, "flavor");
        }
    }

    [CustomEditor(typeof(GameProfileOpt))]
    internal class GameProfileOptInspector : Editor
    {
        private SerializedProperty _gameLanguages  ;
        private SerializedProperty _storeChannels  ;
        private SerializedProperty _channelGateways;
        private SerializedProperty _assetURLs      ;
        private SerializedProperty _patchURLs      ;
        private SerializedProperty _assetFlavors   ;

        private void OnEnable()
        {
            _gameLanguages   = serializedObject.FindProperty("_gameLanguages"  );
            _storeChannels   = serializedObject.FindProperty("_storeChannels"  );
            _channelGateways = serializedObject.FindProperty("_channelGateways");
            _assetURLs       = serializedObject.FindProperty("_assetURLs"      );
            _patchURLs       = serializedObject.FindProperty("_patchURLs"      );
            _assetFlavors    = serializedObject.FindProperty("_assetFlavors"   );
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(_gameLanguages  , new GUIContent(I18N.GameLanguages  ));
            EditorGUILayout.PropertyField(_storeChannels  , new GUIContent(I18N.StoreChannels  ));
            EditorGUILayout.PropertyField(_channelGateways, new GUIContent(I18N.ChannelGateways));
            EditorGUILayout.PropertyField(_assetURLs      , new GUIContent(I18N.AssetURLs      ));
            EditorGUILayout.PropertyField(_patchURLs      , new GUIContent(I18N.PatchURLs      ));
            EditorGUILayout.PropertyField(_assetFlavors   , new GUIContent(I18N.AssetFlavors   ));

            serializedObject.ApplyModifiedProperties();
        }
    }
}
