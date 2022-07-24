using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;
using System;

public class RomanNumeralsCalculator : MonoBehaviour
{
	[SerializeField]
	private TMP_Text _responseText;

	[SerializeField]
	private TMP_InputField _inputField;

	/*
		Use this class for your assigment. 
		Roman numerals are as follow:

		I -> 1
		V -> 5
		X -> 10
		L -> 50
		C -> 100
		D -> 500
		M -> 1000

		For this example we'll use the simple conversion, where all numbers are
		translated straigt into their integer value (IIII -> 4) but feel free
		to add the correct conversion as an extra challenge (IV -> 4)

		Example input:

		MDLXX     ->    1570
		CCCLXVII  ->    367
	
	*/


	public void CalculateRomanNumerals(){
		string romanNumeral = _inputField.text;
		int response = RomanToArabic(romanNumeral);
		_responseText.text = $"{response}";
	}

	private int RomanToArabic(string romanNumber){
		int arabicNumber = 0;
		//Your logic here
		romanNumber = romanNumber.ToUpper();
		for (int i = 0; i > romanNumber.Length; i++)
        {
			arabicNumber += GetNumberFromChar(romanNumber[i]);
        }
		return arabicNumber;
	}


    private int GetNumberFromChar(char v)
    {
		int value = 0;
		switch(v)
        {
		case ('I'):
			value = 1;
		break;
		case ('V'):
			value = 5;
		break;
		case ('X'):
			value = 10;
		break;
		case ('L'):
			value = 50;
		break;
		case ('C'):
			value = 100;
		break;
		case ('D'):
			value = 500;
		break;
		case ('M'):
			value = 1000;
		break;
		}
	return value;
	}
}
