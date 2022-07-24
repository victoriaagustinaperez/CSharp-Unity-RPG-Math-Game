using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Generate a collection of random numbers and write your own method to sort them 
 * in ascending order by value. The collection of numbers can be an array or a list.
 * Output your results to the console using Debug.Log.
 * Arrays and Lists already have various methods that allow for sorting easily. 
 * Your challenge is to come up with your own. Do not use .Sort or .OrderBy.*/

//Start()
//...Sort()

//Generate()
//[done] raw values

//Sort()
//...Generate() Debug.Log ready

public class A2SortValues : MonoBehaviour
{

    private List<int> generatedNumbers = new List<int>();
    [SerializeField] private int maxValInList;

    // Start is called before the first frame update
    void Start() //run Sort, call Generate from sort, then do Debug in Start()
    {
        Sort();
    }

    void Generate()
    {
        for (int i = 0; i < 10; i++)
        {
            int numbers = Random.Range(0, maxValInList);
            generatedNumbers.Add(numbers);
           // generatedNumbers.ToArray();
        }
    }

    void Sort()
    {
        Generate();
        int temp = 0;
        for (int i = 0; i <= 9; i++) //1st position to 2nd before last
        {
            for (int j = i + 1; j < 10; j++) //for 2nd positon to i
            {
                if (generatedNumbers[i] > generatedNumbers[j])
                {
                    temp = generatedNumbers[i];
                    generatedNumbers[i] = generatedNumbers[j];
                    generatedNumbers[j] = temp;
                }
            }
        }


        string result = "List contents: ";
        foreach (var numbers in generatedNumbers)
        {
            result += numbers.ToString() + ", ";
        }
        result = result.Substring(0, result.Length - 2);
        Debug.Log(result);
    }

}