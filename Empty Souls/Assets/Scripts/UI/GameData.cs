using UnityEngine;
public static class GameData
{
    public static BaseAbilitySO selectedFragment;
    public static CharacterType selectedCharacter = CharacterType.Red;
    public static int selectedCharacterIndex = 0;
    // Ќовое поле дл€ хранени€ индекса выбранного уровн€
    /// <summary>
    /// »ндекс уровн€, выбранного в главном меню.
    /// -1 означает, что уровень не был выбран €вно или используетс€ значение по умолчанию.
    /// </summary>
    public static int SelectedLevelIndex = -1;

    // ќпционально: метод дл€ сброса всех игровых данных к значени€м по умолчанию
    public static void ResetToDefaults()
    {
        selectedFragment = null;
        selectedCharacterIndex = 0;
        selectedCharacter = CharacterType.Red; // или другое значение по умолчанию
        SelectedLevelIndex = -1;
        // —брос других данных, если они есть...
        Debug.Log("GameData сброшены к значени€м по умолчанию.");
    }
}