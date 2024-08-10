using System.Threading.Tasks;
using UnityEngine;

public class FindingMatchViewProvider : AddressableLoader
{
    private const string FindingMatchView = "Finding Match View";

    public async Task<FindingMatchView> LoadFindingMatchView(Transform parent = null)
    {
        FindingMatchView findingMatchView = await Load<FindingMatchView>(FindingMatchView, parent);
        findingMatchView.Initialize();
        return findingMatchView;
    }

    public void UnloadFindingMatchView()
    {
        UnloadCachedGameObject();
    }
}