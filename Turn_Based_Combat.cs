using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turn_Based_Combat : MonoBehaviour
{

    int playerHp = 10;
    int aiHp = 10;

    int aiHpBuffer = 10;

    bool playerDefend = false;
    bool playerHealing = true;
    bool deadlyAttack = false;
    bool enemySurrenders = false;


    int enemiesDefeated = 0;
    int killStreak = 0;

    // Start is called before the first frame update
    void Start()
    {

        Debug.Log(" ATTACK: 1 | HEAL: 2 | DEFEND: 3 | SPECIAL ATTACK: 4");
    }

    // Update is called once per frame
    void Update()
    {

        if(playerHp > 0)
        {

            bool nextTurn = false;

            if(enemySurrenders)
            {
                nextTurn = enemySurrendered();
                enemySurrenders = false;
            }
            else
            {
                nextTurn = playersTurn(); // Checking if Player played already --> returns true when played
            }
             

            if(aiHp < 0)
            {
                enemiesDefeated++;
                killStreak++;
                aiHpBuffer++;
                aiHp = aiHpBuffer;
            }

            if(nextTurn)
            {
                enemysTurn();
            }
        
            
        }
        else
        {
            Debug.Log("GAME OVER");
        }
    }

    public void enemysTurn()
    {
        int[] possibleAttacks = new int[100]; //50x 1(attack), 20x 2(charge), 30x magic


        for (int i = 0; i < 50; i++)
        {
            possibleAttacks[i] = 1; // attack
        }
        for (int k = 50; k < 70; k++)
        {
            possibleAttacks[k] = 2; // charge
        }
        for (int k = 70; k < 100; k++)
        {
            possibleAttacks[k] = 3; // magic
        }

        int random = Random.Range(0, 99);
        int aiTurn = possibleAttacks[random];

        // possible moves by AI

        if(aiHp == 1) // AI surrenders if HP is at exactly 1 --> Attack for Killstreak or let him go for 2 HP
        {
            enemySurrenders = true;
            Debug.Log("The Enemy surrenders! 1 : Attack him & get Kill for your Kill Streak | 2: Let him go & receive 2HP.");

        }
        else // normal Gameplay
        {
            switch (aiTurn)
            {
                case 1: // Attack
                    int attack = Random.Range(2, 5);
                    if (playerDefend == false)
                    {
                        playerHp -= attack;
                        Debug.Log("Enemy attacked you.");
                    }
                    else
                    {
                        Debug.Log("Enemy tried to attack you, but you defended");
                    }
                    break;

                case 2: // Charge
                    Debug.Log("Enemy is charging energy!");
                    deadlyAttack = true;
                    break;

                case 3: // Magic
                    Debug.Log("Enemy used Magic.");
                    aiHp++;
                    playerHealing = false;
                    break;
                default:
                    Debug.LogError("Attack not found");
                    break;

            }
        }
        

    }






    bool playersTurn()
    {
        bool played = true;

        playerDefend = false;
      
        if (Input.GetKeyDown(KeyCode.Alpha1)) // Attack : 1
        {
            int damage = Random.Range(1, 3);
            aiHp -= damage;

            Debug.Log("You attacked the Enemy.");
            printStats();

           

        }
        else if (Input.GetKeyDown(KeyCode.Alpha2)) // Healing : 2
        {
            if (playerHp <= 8 && playerHealing == true)
            {
                playerHp = playerHp + 2;
                Debug.Log("You healed yourself.");
                printStats();

               
            }
            else if(playerHealing == false)
            {
                Debug.Log("Enemy used Magic, You can't heal yourself.");
                printStats();
                
            }


        }
        else if (Input.GetKeyDown(KeyCode.Alpha3)) // Defending : 3
        {
            playerDefend = true;
            Debug.Log("Opponents next Attack will be defended.");
            printStats();

            
        }
        else if(Input.GetKeyDown(KeyCode.Alpha4)) // Instant Kill : 4
        {
            if(killStreak >= 3)
            {
                aiHpBuffer++;
                aiHp = aiHpBuffer;
                enemiesDefeated++;
                killStreak = 0;
                Debug.Log("You used your Special Attack. The Enemy is dead.");
                
            }
            else
            {
                Debug.Log("Your Kill Streak is not high enough. You have wasted your Move.");
            }
            
            

        }
        else
        {
            played = false;
        }

        
        if(played) // Checking if Player chose right Move and defended deadly Attack
        {
            // deadly attack 
            if (playerDefend == false && deadlyAttack == true)
            {
                Debug.Log("The Enemy killed you!");
                playerHp = 0;
            }
            else if (playerDefend == true && deadlyAttack == true) // successfully defended
            {
                deadlyAttack = false;
                Debug.Log("Successfully defended deadly Attack.");
            }
        }

        


        return played;
    }

    bool enemySurrendered()
    {
        bool played = true;

        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            
            enemiesDefeated++;
            killStreak++;
            aiHpBuffer++;
            aiHp = aiHpBuffer;
            Debug.Log("You killed the Enemy.");
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            enemiesDefeated++;
            aiHpBuffer++;
            aiHp = aiHpBuffer;
            playerHp = playerHp + 2;
            Debug.Log("You let the Enemy go. You receive 2 HP.");
            
        }
        else
        {
            played = false;
        }

        return played;
    }

    void printStats()
    {
        Debug.Log("Player HP: " + playerHp + " | " + "Enemy HP: " + aiHp + " | Enemies defeated: " + enemiesDefeated + " | Kill Streak: " + killStreak);
    }
}
