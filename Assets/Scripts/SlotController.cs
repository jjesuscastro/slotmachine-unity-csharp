#define LOG_ON
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Text;

public class SlotController : MonoBehaviour
{
    const int COLUMN_COUNT = 5;
    const int ROW_COUNT = 3;

    [Header("Player")]
    [SerializeField]
    Player player;
    [SerializeField]
    PlayerCoins playerCoins;

    [Header("Reel")]
    [SerializeField]
    int[][] reelModels = new int[COLUMN_COUNT][];
    [SerializeField]
    ReelView[] reelViews = new ReelView[COLUMN_COUNT];
    int stoppedReels = 5;

    [Header("Bets")]
    [SerializeField]
    Bets bets;

    public int betIncrement = 500;
    [SerializeField]
    int baseBet = 500;
    int totalBet = 0;

    [Header("Payout Lines")]
    [SerializeField]
    PayoutLines payoutLines;
    [SerializeField]
    Transform payoutLineView;
    [SerializeField]
    GameObject payoutLineViewPrefab;
    List<GameObject> winningPayoutLines = new List<GameObject>();

    [Header("Winnings")]
    [SerializeField]
    TotalWin totalWin;
    int totalPayout = 0;

    [Header("Audio")]
    [SerializeField]
    AudioClip spinAudio;
    [SerializeField]
    AudioClip spinError;
    [SerializeField]
    AudioClip win;

    int[] target = new int[COLUMN_COUNT];
    int[][] spinResult = new int[ROW_COUNT][];

    void Start()
    {
        reelModels[0] = new int[] { 0, 1, 2, 3, 4, 5, 15, 14, 13, 12, 11, 10, 6, 7, 8, 9 };
        reelModels[1] = new int[] { 5, 4, 3, 2, 1, 10, 9, 8, 7, 6, 11, 14, 13, 12, 15, 0 };
        reelModels[2] = new int[] { 5, 1, 4, 3, 2, 6, 7, 8, 9, 10, 15, 14, 13, 12, 11, 0 };
        reelModels[3] = new int[] { 1, 5, 3, 2, 4, 15, 14, 12, 11, 13, 9, 6, 7, 10, 8, 0 };
        reelModels[4] = new int[] { 6, 7, 8, 9, 0, 15, 10, 12, 1, 14, 4, 2, 3, 11, 13, 5 };

        for (int i = 0; i < COLUMN_COUNT; i++)
        {
            reelViews[i].SetSymbols(reelModels[i]);
            reelViews[i].onReelStop.AddListener(SpinStop);
        }

        for (int i = 0; i < ROW_COUNT; i++)
        {
            spinResult[i] = new int[COLUMN_COUNT];
        }

        DisplayPayoutLines();
        totalBet = baseBet * payoutLines.GetPayoutLinesCount();
        bets.UpdateTotalBetAmount(totalBet);
        playerCoins.UpdatePlayerCoins(player.GetCoins());
    }

    void DisplayPayoutLines()
    {
        for (int i = 0; i < payoutLines.GetPayoutLinesCount(); i++)
        {
            PayoutLineView pLineView = Instantiate(payoutLineViewPrefab, Vector3.zero, Quaternion.identity, payoutLineView).GetComponent<PayoutLineView>();
            pLineView.SetPayoutLine(payoutLines.GetPayoutLines()[i]);
        }
    }

    int GetRandomSymbol(int[] reel)
    {
        int randomIndex = Random.Range(1, reel.Length - 2);
        return randomIndex;
    }

    public void IncreaseBet()
    {
        baseBet = Mathf.Clamp(baseBet += betIncrement, 1, 10000);
        totalBet = baseBet * payoutLines.GetPayoutLinesCount();
        bets.UpdateTotalBetAmount(totalBet);
    }

    public void DecreaseBet()
    {
        baseBet = Mathf.Clamp(baseBet -= betIncrement, 1, 10000);
        totalBet = baseBet * payoutLines.GetPayoutLinesCount();
        bets.UpdateTotalBetAmount(totalBet);
    }

    void SpinStop()
    {
        stoppedReels++;

        if (stoppedReels == 5)
        {
            if (totalPayout > 0)
            {
                player.AddCoins(totalPayout);
                totalWin.UpdateTotalWin(totalPayout);
                playerCoins.UpdatePlayerCoins(player.GetCoins());
                AudioSource.PlayClipAtPoint(win, Vector3.zero);

                foreach (GameObject winningLine in winningPayoutLines)
                    winningLine.SetActive(true);
            }
        }
    }

    public void Spin()
    {
        totalWin.UpdateTotalWin(0);
        if (stoppedReels == 5)
        {
            if (player.SubtractCoins(totalBet))
            {
                winningPayoutLines.Clear();
                stoppedReels = 0;
                AudioSource.PlayClipAtPoint(spinAudio, Vector3.zero);

                for (int i = 0; i < COLUMN_COUNT; i++)
                {
                    target[i] = GetRandomSymbol(reelModels[i]);
                    reelViews[i].Spin(10000 + (i * 5000), target[i]);
                }

                // Debug.Log("targets " + string.Join(", ", target));

                GetSpinResult(target, reelModels);

                playerCoins.UpdatePlayerCoins(player.GetCoins());
            }
            else
            {
                AudioSource.PlayClipAtPoint(spinError, Vector3.zero);
            }
        }
        else
        {
            for (int i = 0; i < COLUMN_COUNT; i++)
            {
                reelViews[i].StopSpin();
            }

            stoppedReels = 5;
        }
    }

    void GetSpinResult(int[] target, int[][] reelModels)
    {
        for (int i = 0; i < COLUMN_COUNT; i++)
        {
            int result = target[i];
            int reelLength = reelModels[i].Length;
            spinResult[0][i] = (result - 1 >= 0) ? reelModels[i][result - 1] : reelModels[i][reelLength - 1];
            spinResult[1][i] = reelModels[i][result];
            spinResult[2][i] = (result + 1 >= reelLength) ? reelModels[i][0] : reelModels[i][result + 1];

        }

        CheckPayout(spinResult);
    }

    void CheckPayout(int[][] spinResult)
    {
        SymbolsCatalog symbolsCatalog = SymbolsCatalog.i;
        List<Symbol> symbols = symbolsCatalog.GetSymbols();
        int payoutLinesCount = payoutLines.GetPayoutLinesCount();
        int[] symbolPayouts = new int[symbols.Count];
        int[][] pLines = payoutLines.GetPayoutLines();

        for (int i = 0; i < payoutLinesCount; i++)
        {
            PayoutLineView pLineView = payoutLineView.GetChild(i).GetComponent<PayoutLineView>();
            Debug.Log("payouts " + i + " " + string.Join(", ", pLineView.pLine));
        }

        for (int j = 0; j < payoutLinesCount; j++)
        {
            int[] result = new int[COLUMN_COUNT];
            Transform pLineView = payoutLineView.GetChild(j);
            for (int i = 0; i < COLUMN_COUNT; i++)
            {
                result[i] = spinResult[pLines[j][i]][i];
            }

            if (CheckPayoutSymbols(result, symbols, symbolPayouts))
            {
                winningPayoutLines.Add(pLineView.gameObject);
            }
            else
            {
                pLineView.gameObject.SetActive(false);
            }
        }

        totalPayout = symbolPayouts.Sum() * totalBet;
    }

    public void ShowPayoutLines(bool value)
    {
        foreach (Transform pLineView in payoutLineView)
        {
            pLineView.gameObject.SetActive(value);
        }
    }

    bool CheckPayoutSymbols(int[] result, List<Symbol> symbols, int[] symbolPayouts)
    {
        bool returnValue = false;

        for (int i = 0; i < symbols.Count; i++)
        {
            int count = 0;
            for (int j = 0; j < result.Length; j++)
            {

                if (result[j] == symbols[i].id && result[j] == result[0])
                    count++;
                else
                    break;
            }

            int payoutIndex = Mathf.Clamp(count - 1, 0, result.Length - 1);
            symbolPayouts[i] += symbols[i].payout[payoutIndex];
            if (symbols[i].payout[payoutIndex] > 0)
                returnValue = true;
        }

        return returnValue;
    }
}
