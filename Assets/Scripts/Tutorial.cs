using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public Sprite[] sprites;
    Image tutImage;
    int currentSprite = 0;
    int manditoryInteractionPeriod = 59;
    int state = -1;
    
    IEnumerator HandleSprites(){
        tutImage = GetComponent<Image>();
        while (state < 4) {
            for (int i = 0; i < 20; i ++){
                switch (state){
                    default:
                        currentSprite = currentSprite % 4;
                        manditoryInteractionPeriod -= 
                            (Input.GetKey("w") || Input.GetKey("up") || Input.GetKey("i")) ||
                            (Input.GetKey("a") || Input.GetKey("left") || Input.GetKey("j")) || 
                            (Input.GetKey("s") || Input.GetKey("down") || Input.GetKey("k")) ||
                            (Input.GetKey("d") || Input.GetKey("right") || Input.GetKey("l")) ? 2 : 0;
                    break;
                    case (1):
                        currentSprite = (currentSprite - 4) % 2 + 4;
                        manditoryInteractionPeriod -= Input.GetMouseButton(0) || Input.GetKey("e") ? 50 : 0;
                    break;
                    case (2):
                        currentSprite = (currentSprite - 6) % 3 + 6;
                        manditoryInteractionPeriod -= (Input.GetKey("space") || Input.GetKey("enter")) ? 4 : 0;
                    break;
                    case (3):
                        currentSprite = (currentSprite - 9) % 2 + 9;
                        manditoryInteractionPeriod -= 
                            ((Input.GetKey("w") || Input.GetKey("up") || Input.GetKey("i")) ||
                            (Input.GetKey("s") || Input.GetKey("down") || Input.GetKey("k"))) &&
                            (Input.GetKey("space") || Input.GetKey("enter"))? 1 : 0;
                    break;
                }
                yield return new WaitForSeconds(0.04f);
            }
            if (manditoryInteractionPeriod < 0)
                StartCoroutine(SwapState());
            tutImage.sprite = sprites[currentSprite];
            currentSprite ++;
        }
    }

    IEnumerator SwapState(){
        manditoryInteractionPeriod = 999999;
        while (Mathf.Round(transform.position.y) != 0){
            transform.position = new Vector3(transform.position.x, transform.position.y * 2 / 3, 0);
            yield return new WaitForSeconds(0.02f);
        }
        state ++;
        yield return new WaitForSeconds(1.7f);
        if (state == 4)
            Destroy(gameObject);
        while (Mathf.Round(transform.position.y) != 300){
            transform.position = new Vector3(transform.position.x, (transform.position.y * 2 + 300) / 3, 0);
            yield return new WaitForSeconds(0.02f);
        }
        manditoryInteractionPeriod = 59;
    }

    void Start(){
        StartCoroutine(SwapState());
        StartCoroutine(HandleSprites());
    }
}
