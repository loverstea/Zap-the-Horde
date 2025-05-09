using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Gendolf : MonoBehaviour
{
    public TextMeshProUGUI Text;
    public GameObject Tutorial;
    private int Triger;

    public void Update()
    {
        GendolfSay();
    }
    public void GendolfSay()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (!Tutorial.activeSelf)
            {
                Tutorial.SetActive(true);            
            }
            switch (Triger)
            {
                case 0:
                    Text.text = "Вітаю тебе мандрівнику! \r\nМене звуть Гендольф, я чарівник країни Бобрикстан. \r\nІ я тут що б помогти тобі з інструктажом ";
                    Triger++;
                    break;
                case 1:
                    Text.text = "На клавіші W A S D ти можеш пересуватися\r\nShift для бігу, Ctrl для присідань, щоб попу підкачати";
                    Triger++;
                    break;
                case 2:
                    Text.text = "На Space ти можеш стрибати, \r\nпопробуй перестрибнути цю перешкоду";
                    Triger++;
                    break;
                case 3:
                    Text.text = "Спереду тебе стоїть мішень. \r\nНатисни на Q що б вибрати пістолет, ЛКМ що б вистрілити.";
                    Triger++;
                    break;
                case 4:
                    Text.text = "Вибери першу башню. \r\nЦе \"Арбалет\" , він допоможе тобі з захистом від монстрів!";
                    Triger++;
                    break;
                case 5:
                    Text.text = "Бажаю вдачі тобі мандрівнику, \r\nНадіюсь ти зможеш спасти Гризоплавію від цих скажених монстрів";
                    Triger++;
                    break;
                case 6:
                    Tutorial.SetActive(false);
                    break;

            }             
        }
    }
}
