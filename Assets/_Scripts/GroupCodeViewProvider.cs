using System.Threading.Tasks;
using UnityEngine;

public class GroupCodeViewProvider : AddressableLoader
{
    private const string GroupCodeView = "Group Code View";

    public async Task<GroupCodeView> ShowGroupCodeView(string groupCode, Transform parent = null)
    {
        GroupCodeView groupCodeView = await Load<GroupCodeView>(GroupCodeView, parent);
        groupCodeView.Initialize();
        groupCodeView.ShowGroupCode(groupCode);
        return groupCodeView;   
    }

    public void UnloadGroupCodeView()
    {
        UnloadCachedGameObject();
    }
}