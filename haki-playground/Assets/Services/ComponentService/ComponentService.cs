using System.Collections.Generic;
using UnityEngine;

public class ComponentService : MonoBehaviour
{
   // you need a collection of internal components.
   private IEnumerable<object> components;
   // you need to be able to place them in UI
   private object uiElement;
   // you need to be able to select these elements from ui

   void SelectComponent()
   {

   }
   // and place them somewhere in the scene
   void PlaceComponent()
   {

   }
   // perhaps some sort of drag and drop
   void DragAndDrop()
   {

   }
}
