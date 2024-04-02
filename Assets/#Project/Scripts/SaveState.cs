public class SaveState
{
    public int loadFirstTime = 0;

    // Currency
    public int cash = 0;

    // Audio
    public float soundFxVolume = 0.75f;
    public float backgroundVolume = 0.3f;

    // Controls
    public int autoShoot = 1;
    public float controlSensitivity = 1.5f;//2.3

    // Weapons
    public int weaponUnlocked = 0;
    public int weaponEquipped = 0;
    public int selectedWeaponIndex = 0;

    // Misc.
    public int nextButtonPressed = 0;
    public int playAgainButtonPressed = 0;
    public int gamePlayLoadoutButtonPressed = 0;
    public int nameEntryPanelSeen = 0;
    public int isRated = 0;
    public int medicKit = 0;
    public int grenade = 0;
    public int invulnerable = 0;
    public int count = 0;
    public float sprintSpeed = 7.5f;
}