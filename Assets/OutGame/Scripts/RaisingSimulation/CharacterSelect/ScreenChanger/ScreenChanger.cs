using VContainer;

public class ScreenChanger
{
    private CharacterPickController _characterPickController;
    private SupportCardSelectController _supportCardSelectController;

    [Inject]
    public ScreenChanger(CharacterPickController characterPickController, SupportCardSelectController supportCardSelectController)
    {
        this._characterPickController = characterPickController;
        this._supportCardSelectController = supportCardSelectController;

        ShowCharacterPickScreen();
    }

    public void ShowCharacterPickScreen()
    {
        _characterPickController.CharacterPickUIHolder.ViewCanvasObj.SetActive(true);
        _characterPickController.gameObject.SetActive(true);
        _supportCardSelectController.SupportCardSelectUIHolder.ViewCanvasObj.SetActive(false);
        _supportCardSelectController.gameObject.SetActive(false);
    }

    public void ShowSupportCardPickScreen()
    {
        _supportCardSelectController.SupportCardSelectUIHolder.ViewCanvasObj.SetActive(true);
        _supportCardSelectController.gameObject.SetActive(true);
        _characterPickController.CharacterPickUIHolder.ViewCanvasObj.SetActive(false);
        _characterPickController.gameObject.SetActive(false);
    }
}
