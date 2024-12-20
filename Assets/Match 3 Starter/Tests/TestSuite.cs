using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;



public class TestSuite
{
    private GameObject boardObject;
    private GameObject guiObject;
    private GameObject gameManagerObject;
    private BoardManager boardManager;
    private GUIManager guiManager;
    private GameManager gameManager;
    private GameObject tilePrefab;

    /// Metodo que se ejecuta al comienzo de cada test
    [SetUp]
    public void SetUp()
    {

        // Crear GameManager primero
        gameManagerObject = new GameObject("GameManager");
        gameManager = gameManagerObject.AddComponent<GameManager>();
        GameManager.instance = gameManager;

        // Crear el prefab de la ficha
        tilePrefab = new GameObject("TilePrefab");
        tilePrefab.AddComponent<RectTransform>();
        tilePrefab.AddComponent<Image>();
        tilePrefab.AddComponent<Button>();
        tilePrefab.AddComponent<Tile>();

        // Configurar BoardManager
        boardObject = new GameObject("BoardManager");
        boardManager = boardObject.AddComponent<BoardManager>();
        BoardManager.instance = boardManager;
        boardManager.tilePrefab = tilePrefab;
        boardManager.xSize = 3;
        boardManager.ySize = 3;
        boardManager.candies = new List<Sprite>();

        // Crear sprites basicos
        for (int i = 0; i < 6; i++)
        {
            Texture2D texture = new Texture2D(32, 32);
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, 32, 32), Vector2.one * 0.5f);
            boardManager.candies.Add(sprite);
        }

        // Configurar GUIManager con referencias mInimas
        guiObject = new GameObject("GUIManager");
        guiManager = guiObject.AddComponent<GUIManager>();
        GUIManager.instance = guiManager;
        
    }

    /// Metodo que se ejecuta al final de cada test
    [TearDown]
    public void TearDown()
    {
        // Limpieza normal
        Object.DestroyImmediate(gameManagerObject);
        Object.DestroyImmediate(boardObject);
        Object.DestroyImmediate(guiObject);
        Object.DestroyImmediate(tilePrefab);

        // Limpiar PlayerPrefs usados en las pruebas
        PlayerPrefs.DeleteKey("CurrentScore");
        PlayerPrefs.DeleteKey("HighScore");
        PlayerPrefs.Save();
    }


    /// Test1: Que comprueba si el combinar 4 caramelos en hilera suma al puntaje del jugador
    [UnityTest]
    public IEnumerator Test_FourTileSpecialCombination()
    {
        // Arrange
        Dictionary<string, int> candyMap = new Dictionary<string, int>()
        {
            {"R", 0}, {"B", 1}, {"G", 2},
            {"M", 3}, {"P", 4}, {"Y", 5}
        };

        string[,] gridInicial = new string[,] {
            {"B", "B", "B", "B"},
            {"R", "Y", "M", "P"},
            {"G", "P", "B", "Y"}
        };
        //tamaño 3 4
        boardManager.xSize = 4; 
        boardManager.ySize = 3;

        Sprite[,] spriteGrid = new Sprite[4, 3];
        for (int x = 0; x < 4; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                spriteGrid[x, y] = boardManager.candies[candyMap[gridInicial[y, x]]];
            }
        }
        // Act
        boardManager.InitializeBoard(spriteGrid);
        yield return null;

        var matches = boardManager.FindMatches(new Vector2Int(0, 0)); //compara
        // Assert
        Assert.AreEqual(4, matches.Count, "Deberia encontrar 4 fichas coincidentes en horizontal"); //Comprueba y confirma q hay 4 combis
    }

    // Test 2: Verificar si no hay combinaciones disponibles
    [UnityTest]
    public IEnumerator Test_NoAvailableMatches()
    {
        // Arrange
        Dictionary<string, int> candyMap = new Dictionary<string, int>()
    {
        {"R", 0}, {"B", 1}, {"G", 2},
        {"M", 3}, {"P", 4}, {"Y", 5}
    };
        // Un tablero sin combinaciones posibles
        string[,] gridInicial = new string[,] {
        {"R", "G", "B"},
        {"P", "M", "Y"},
        {"B", "Y", "G"}
    };
        boardManager.xSize = 3;
        boardManager.ySize = 3;

        Sprite[,] spriteGrid = new Sprite[3, 3];
        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                spriteGrid[x, y] = boardManager.candies[candyMap[gridInicial[y, x]]];
            }
        }
        // Act
        boardManager.InitializeBoard(spriteGrid);
        yield return null;
        // Verificar si hay combinaciones disponibles
        var matches = boardManager.FindMatches(new Vector2Int(0, 0));
        // Assert
        Assert.AreEqual(0, matches.Count, "No debería haber combinaciones disponibles.");
    }


    // Test 3: Verificar el comportamiento del botón "Play"
    [UnityTest]
    public IEnumerator PlayButton_LoadsGameScene()
    {
        // Cargar la escena de menu
        SceneManager.LoadScene("Menu");
        yield return null;

        // Buscar el boton 'PlayButton'
        GameObject playButton = GameObject.Find("PlayButton");
        Assert.IsNotNull(playButton, "El botón 'PlayButton' no se encontro en la escena.");

        // Asegúrate de que el componente Button está presente
        Button buttonComponent = playButton.GetComponent<Button>();
        Assert.IsNotNull(buttonComponent, "El componente Button no esta presente en el objeto 'PlayButton'.");

        // Simular el clic en el botón 'PlayButton'
        buttonComponent.onClick.Invoke();
        yield return new WaitForSeconds(2f);

        // Comprobar que la escena activa es "Game"
        Assert.AreEqual("Game", SceneManager.GetActiveScene().name, "La escena no cambio a 'Game' al presionar el boton.");
    }

}
