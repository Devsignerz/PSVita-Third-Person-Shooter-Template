using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

	//ui items
	public GameObject[] UIItems;
	//debug text
	public Text d;
	[SerializeField] private int i = 0;
	[SerializeField] private Color selectedColor;
	[SerializeField] private Color deselectedColor;

	// Update is called once per frame
	void Update(){
		//up
		if(Input.GetKeyDown(KeyCode.JoystickButton8)){
			if(i>0){
				i--;
				MoveToItem(i+1, i);
			}
		}
		//down
		if(Input.GetKeyDown(KeyCode.JoystickButton10)){
			if(i<UIItems.Length-1){
				i++;
				MoveToItem(i-1, i);
			}
		}
		//x
		if(Input.GetKeyDown(KeyCode.JoystickButton0)){
			UIItems[i].GetComponent<Button>().onClick.Invoke();
		}
		//circle
		if(Input.GetKeyDown(KeyCode.JoystickButton1)){
		}
		d.text = i.ToString();
	}
	//c = current index, p = previous index
	void MoveToItem(int p, int c){
		UIItems[p].GetComponent<Text>().color = deselectedColor;
		UIItems[c].GetComponent<Text>().color = selectedColor;
	}
}
