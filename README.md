# TP-Final-Minitrope-Exilia2
**[Documento](https://docs.google.com/document/d/1TrZ_DIOsMb3A-tq-7ddZgGXpnEocvC2q/edit?usp=sharing&ouid=104358231682201099483&rtpof=true&sd=true)**

## 1. Técnicas de Testing

### Técnicas de Testing para Minitrope (similar a Candy Crush):
- **Partición de Equivalencia**:
  - Probar combinaciones válidas e inválidas de movimientos y configuraciones iniciales.
  - Ejemplo: Validar que los movimientos dentro del límite permitido (5) sean aceptados, y que cualquier acción.

- **Análisis de Valores Límite**:
  - Evaluar los límites de puntuación y movimientos.
  - Ejemplo:
    - El jugador logra 0 puntos (no se combina nada).
    - El jugador combina en todos los movimientos posibles.

- **Testing Negativo**:
  - Probar acciones no válidas, como intentar mover caramelos fuera del tablero o hacer movimientos después de agotar los 5 límites permitidos.
  - Ejemplo:
    - Verificar que el sistema no permite un movimiento diagonal no válido.

- **Testing Combinatorio (Pairwise)**:
  - Probar combinaciones de caramelos en distintas posiciones iniciales del tablero.
  - Ejemplo: Validar que todas las combinaciones posibles de caramelos generan el puntaje esperado.

---

## 2. Casos de Prueba

### Casos de Prueba para el Juego de Minitrope

| ID    | Funcionalidad                                                         | Pasos para reproducir                                                                                                               | Resultado esperado                                                                                 | Resultado obtenido                                                              |
|-------|-----------------------------------------------------------------------|-------------------------------------------------------------------------------------------------------------------------------------|---------------------------------------------------------------------------------------------------|--------------------------------------------------------------------------------|
| 0001  | Iniciar Partida                                                       | En el menú de juego, presionar el botón "Play"                                                                                       | Transición hacia la escena del nivel                                                                | Se cargó el nivel del juego                                                    |
| 0002  | Seleccionar un caramelo                                                | Iniciar el nivel. Hacer clic sobre un caramelo.                                                                                      | Cambiar la apariencia del Sprite.                                                                  | El caramelo se torna de una tonalidad oscura.                                   |
| 0003  | Intercambiar un caramelo en dirección horizontal y vertical            | Iniciar el nivel. Hacer clic sobre un caramelo. Hacer clic sobre uno de los caramelos adyacentes.                                    | El primer caramelo seleccionado debe intercambiar de lugar con el segundo.                         | Los caramelos intercambian de lugar.                                            |
| 0004  | Destruir una combinación de tres o más caramelos                       | Iniciar el nivel. Hacer clic sobre un caramelo. Hacer clic sobre uno de los caramelos adyacentes. Formar una fila o columna de 3 o más caramelos. | La combinación de estos caramelos debe destruirse.                                                 | La combinación de caramelos se destruye correctamente.                          |
| 0005  | Las combinaciones accidentales de tres o más caramelos consecutivos en filas o columnas se destruyen | Iniciar el nivel. Hacer clic sobre un caramelo. Hacer clic sobre uno de los caramelos adyacentes. Destruir caramelos formando combinaciones de tres o más en filas o columnas. Esperar que se agreguen caramelos faltantes, se acomoden y se formen nuevas combinaciones de tres o más. | La combinación de caramelos que se destruyan debe ser correcta y las nuevas combinaciones deben formarse adecuadamente. | Las combinaciones de 3 o más caramelos en fila o columna no siempre se destruyen como se esperaba. |


---

## 3. Implementación de Pruebas Unitarias

Las pruebas se implementaron utilizando **NUnit** y el **Unity Test Framework** para garantizar la calidad y el correcto funcionamiento del juego.

### Juego de Minitrope
A continuación, se describen las pruebas unitarias realizadas.

---

## Tests Implementados

### 1. Test_FourTileSpecialCombination
#### Descripción
`Test que comprueba si el combinar 4 caramelos en hilera suma al puntaje del jugador`.

```csharp
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

        var matches = boardManager.FindMatches(new Vector2Int(0, 0));

        // Assert
        Assert.AreEqual(4, matches.Count, "Deberia encontrar 4 fichas coincidentes en horizontal");
    }
```

---

### 2. Test_NoAvailableMatches
#### Descripción
`Test 2: Verificar si no hay combinaciones disponibles`.

```csharp
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

```

---

### 3. PlayButton_LoadsGameScene
#### Descripción
`Test 4: Verificar el comportamiento del botón "Play"`.

```csharp
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
        Assert.IsNotNull(buttonComponent, "El componente Button no está presente en el objeto 'PlayButton'.");

        // Simular el clic en el botón 'PlayButton'
        buttonComponent.onClick.Invoke();
        yield return new WaitForSeconds(2f);

        // Comprobar que la escena activa es "Game"
        Assert.AreEqual("Game", SceneManager.GetActiveScene().name, "La escena no cambió a 'Game' al presionar el boton.");
    }
```
