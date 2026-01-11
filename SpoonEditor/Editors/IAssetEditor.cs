using SpoonEditor.Content;

namespace SpoonEditor.Editors
{
	interface IAssetEditor
	{
		Asset Asset { get; }

		void SetAsset(AssetInfo asset);
	}
}
