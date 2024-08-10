using UnityAnimator = UnityEngine.Animator;

public sealed class Constants
{
    public const byte MinFramerate = 30;
    public const short MaxFramerate = 600;

    public const string SettingsFileName = "Settings";
    public const string AvatarsDataFileName = "Avatars Data";

    public const byte MaxPlayers = 2;

    public sealed class Localization
    {
        public sealed class TableReferences
        {
            public const string MainMenuTable = "Main Menu Table";
            public const string SettingsMenuTable = "Settings Menu Table";
            public const string ProfileTable = "Profile Table";
        }

        public sealed class TableEntryReferences
        {
            public const string Confirm = "Confirm"; 
            public const string Cancel = "Cancel"; 
        }
    }

    public sealed class CloudSave
    {
        public sealed class Keys
        {
            public const string PlayerId = "PlayerId";
            public const string PlayerName = "PlayerName";
            public const string PlayerAvatarId = "PlayerAvatarId";
        }
    }

    public sealed class Scenes
    {
        public const string MainMenu = "Main Menu Scene";
        public const string FightArena = "Fight Arena Scene";
        public const string Training = "Training Scene";
    }

    public sealed class Addressables
    {
        public sealed class Keys
        {
            public const string PauseMenu = "Pause Menu";
            public const string SettingsMenu = "Settings Menu";
            public const string PlayerNameInputWindow = "Player Name Input Window";
            public const string AddFriendInputWindow = "Add Friend Input Window";
            public const string GroupCodeInputWindow = "Group Code Input Window";
            public const string PlayerFeatures = "Player Features";
            public const string HoverHintView = "Hover Hint View";
            public const string MemberOfGroupProfileView = "Member Of Group Profile View";
            public const string PopupMessage = "Popup Message";
        }
    }

    public sealed class Direction
    {
        public const sbyte Left = -1;
        public const byte Zero = 0;
        public const byte Right = 1;
    }

    public sealed class Animator
    {
        public sealed class Names
        {
            public const string Empty = "Empty";
            public const string Idle = "Idle";
            public const string SinglePunch = "Single Punch";
            public const string Crouch = "Crouch";
            public const string StandUp = "Stand Up";
        }

        public sealed class Params
        {
            public static readonly int CaracterMoveDirection = UnityAnimator.StringToHash("Character Move Direction");
            public static readonly int IsCharacterGrounded = UnityAnimator.StringToHash("Is Character Grounded");
            public static readonly int IsCharacterInBlock = UnityAnimator.StringToHash("Is Character In Block");
            public static readonly int IsCharacterCrouching = UnityAnimator.StringToHash("Is Character Crouching");
            public static readonly int IsCharacterRunning = UnityAnimator.StringToHash("Is Character Running");
            public static readonly int IsCharacterUsingUppercutFromCrouch = UnityAnimator.StringToHash("Is Character Using Uppercut From Crouch");
            public static readonly int SinglePunchTrigger = UnityAnimator.StringToHash("Single Punch Trigger");
            public static readonly int UppercutFromIdleTrigger = UnityAnimator.StringToHash("Uppercut From Idle Trigger");
            public static readonly int UppercutFromCrouchTrigger = UnityAnimator.StringToHash("Uppercut From Crouch Trigger");
            public static readonly int SweepTrigger = UnityAnimator.StringToHash("Sweep Trigger");
            public static readonly int SweepFallTrigger = UnityAnimator.StringToHash("Sweep Fall Trigger");
            public static readonly int ReceivePunchTrigger = UnityAnimator.StringToHash("Receive Punch Trigger");
            public static readonly int ReceiveUppercutTrigger = UnityAnimator.StringToHash("Receive Uppercut Trigger");
            public static readonly int JumpTrigger = UnityAnimator.StringToHash("Jump Trigger");
            public static readonly int LegKickTrigger = UnityAnimator.StringToHash("Leg Kick Trigger");
            public static readonly int StepBackwardTrigger = UnityAnimator.StringToHash("Step Backward Trigger");
            public static readonly int StepForwardTrigger = UnityAnimator.StringToHash("Step Forward Trigger");
            public static readonly int KnockoutBackwardTrigger = UnityAnimator.StringToHash("Knockout Backward Trigger");
        }

        public sealed class Layers
        {
            public const int BaseLayer = 0;
            public const int UpperBodyLayer = 1;
        }
    }
}