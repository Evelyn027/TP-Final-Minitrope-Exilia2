using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.TestTools;

namespace Tests
{
    public class PlayButtonTest
    {
        [UnityTest]
        public IEnumerator PlayButton_LoadsGameScene()
        {
            // Cargar la escena de menu
            SceneManager.LoadScene("Menu");
            yield return null; // Esperar un frame para que la escena se cargue completamente

            // Buscar el botón 'PlayButton' en la escena
            GameObject playButton = GameObject.Find("PlayButton");
            Assert.IsNotNull(playButton, "El botón 'PlayButton' no se encontro en la escena.");

            // Asegúrate de que el componente Button esta presente
            Button buttonComponent = playButton.GetComponent<Button>();
            Assert.IsNotNull(buttonComponent, "El componente Button no esta presente en el objeto 'PlayButton'.");

            // Simular el clic en el botón 'PlayButton'
            buttonComponent.onClick.Invoke();
            yield return new WaitForSeconds(2f); // Esperar un poco para asegurarnos que el cambio de escena ocurre

            // Comprobar que la escena activa es "Game"
            Assert.AreEqual("Game", SceneManager.GetActiveScene().name, "La escena no cambió a 'Game' al presionar el botón.");
        }
    }
}
