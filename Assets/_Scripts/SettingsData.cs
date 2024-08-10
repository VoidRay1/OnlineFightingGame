using System;

[Serializable]
public class SettingsData
{
    public byte ResolutionIndex;
    public short Framerate;
    public byte QualityPresetIndex;
    public bool IsFullScreenMode;
    public bool IsVsyncEnabled;
    public float Volume;
    public byte LanguageIndex;

    public SettingsData ShallowCopy()
    {
        return (SettingsData)MemberwiseClone();
    }
}