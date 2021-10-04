using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    public string titleScene;
    public Camera mainCamera;

    Animator animator;
    BoxCollider2D button;
    Vector2 mousePos;
    bool active = false;


    void Start()
    {
        button = GetComponentInChildren<BoxCollider2D>();
        animator = GetComponentInChildren<Animator>();
    }

    public void GameOver() {
        animator.SetTrigger("Game Over");
        active = true;
    }

    IEnumerator LoadTitleScene(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    public void OnClick() {
        StartCoroutine(LoadTitleScene(titleScene));
    }


    // Update is called once per frame
    void Update()
    {
        if (active) {
            mousePos = mainCamera.ScreenToWorldPoint((Vector2)Mouse.current.position.ReadValue());
        }
    }
}
