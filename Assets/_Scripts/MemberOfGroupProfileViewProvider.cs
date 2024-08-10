using System.Threading.Tasks;
using UnityEngine;

public class MemberOfGroupProfileViewProvider : AddressableLoader
{
    private MemberOfGroupProfileView _memberOfGroupProfileView;

    public async Task<MemberOfGroupProfileView> ShowMemberOfGroupProfileView(PlayerData playerData, Transform parent)
    {
        _memberOfGroupProfileView = await Load<MemberOfGroupProfileView>(Constants.Addressables.Keys.MemberOfGroupProfileView, parent);
        _memberOfGroupProfileView.Initialize(playerData);
        _memberOfGroupProfileView.Show();
        return _memberOfGroupProfileView;
    }

    public void UnloadMemberOfGroupProfileView()
    {
        _memberOfGroupProfileView = null;
        UnloadCachedGameObject();
    }
}