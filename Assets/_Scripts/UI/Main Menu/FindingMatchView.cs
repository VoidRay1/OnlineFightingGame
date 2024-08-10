using System;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.SmartFormat.PersistentVariables;

public class FindingMatchView : BaseView
{
    [SerializeField] private TMP_Text _findingMatchTimeText;
    [SerializeField] private LocalizedString _localizedFindingMatchTimeText;

    private const string FindingMatchTime = "FindingMatchTime";

    public void Initialize()
    {
/*        _localizedFindingMatchTimeText.Add(FindingMatchTime, new IntVariable());
        _localizedFindingMatchTimeText.StringChanged += FindingMatchTimeChanged;*/
    }

    public void DisplayTime(int time)
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(time);
        _findingMatchTimeText.text = "Finding Match: " + string.Format($"{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}");
/*        (_localizedFindingMatchTimeText[FindingMatchTime] as IntVariable).Value = time;*/
        base.Show();
    }

    private void FindingMatchTimeChanged(string value)
    {
        _findingMatchTimeText.text = value;
    }

    private void OnDestroy()
    {
       /* _localizedFindingMatchTimeText.StringChanged -= FindingMatchTimeChanged;*/
    }
}