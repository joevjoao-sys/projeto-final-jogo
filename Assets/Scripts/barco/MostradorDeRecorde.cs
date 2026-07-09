using UnityEngine;
using TMPro;

public class MostradorDeRecorde : MonoBehaviour
{
    [Header("Arraste o texto aqui")]
    public TextMeshProUGUI textoNaTela;

    void OnEnable()
    {
        // 1. Vê em qual onda o Capitão morreu
        WaveManager waveManager = FindObjectOfType<WaveManager>();
        int ondaDaMorte = 1;

        if (waveManager != null)
        {
            ondaDaMorte = waveManager.waveAtual;
        }

        // 2. Puxa o recorde histórico salvo no PC (Mudei o nome interno para "RecordeRealPirata" para limpar o bug antigo)
        int maiorRecorde = PlayerPrefs.GetInt("RecordeRealPirata", 1);

        // 3. Verifica se o jogador bateu o próprio recorde AGORA
        if (ondaDaMorte > maiorRecorde)
        {
            // Bateu o recorde! Salva o novo valor no PC.
            PlayerPrefs.SetInt("RecordeRealPirata", ondaDaMorte);
            PlayerPrefs.Save();

            // Mostra uma mensagem especial de vitória
            if (textoNaTela != null)
            {
                textoNaTela.text = "NOVO RECORDE ALCANÇADO!\nVOCÊ SOBREVIVEU ATÉ A ONDA " + ondaDaMorte;
            }
        }
        else
        {
            // Não bateu o recorde, então mostra a comparação normal
            if (textoNaTela != null)
            {
                textoNaTela.text = "ONDA ATUAL: " + ondaDaMorte + "\n" +
                                   "MAIOR RECORDE: ONDA " + maiorRecorde;
            }
        }
    }
}