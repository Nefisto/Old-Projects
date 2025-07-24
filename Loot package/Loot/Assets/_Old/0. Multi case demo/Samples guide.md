## Summary

* [Overview](#overview)
* [Simple drop](#simple-drop)
* [Weighted drop](#Weighted-drop)
* [Table drop](#Table-drop)
* [Game Object drop](#Game-Object-drop)
* [Custom drop](#custom-drop)
* [Drop with removal](#drop-with-removal)
* [Repeatable drop](#Repeatable-drop)
* [Local and temporary modifiers](#Local-and-temporary-modifiers)
* [Custom logic](#Custom-logic)
* [Drop with global modifier](#Drop-with-global-modifier)

---

## [Overview](#summary)


All samples have a CaseData that is my try to simulate a monster data, in sample this only store information that will be show in header and the DropTable, but in a real scenario it will any have any amount of information AND a DropTable.
I have created a simple hierarchy (as you can see bellow) to allow us to modifier and filter things based on type, this is an effort to simulate an real scenario.

- Scriptable Object
  - Item
    - Currency
    - Consumables
    - Misc

In all cases dropped items will appear in the console.

---

## [Simple drop](#summary)

The normal case, each item have their own percentage, so we can end with multiple items even calling for Drop only one time.

**Examples:** 

* You have killed a monster
* Will drop a bunch of items at same time in the end of a dungeon

---

## [Weighted drop](#summary)

Another normal case but drops are weighted, which means that their percentage is based on the weight of whole table.

**Examples:**

* You have killed a monster
* Will drop a bunch of items at same time in the end of a dungeon

---

## [Table drop](#summary)

How to setup tables that drop other tables.

**Examples:**

* You drop packs that drop a subgroup of items including another packs

---

## [Game Object drop](#summary)

Similar to first case but sometimes you simple want to drop prefabs to instantiate on runtime

**Examples:**

* Instantiate a custom goblin in predefined intervals

---

## [Custom drop](#summary)

It's a common thing to desire some kind of custom behavior when calling for drop, here I try to show how to apply filters to our table.

**Example:** 

* After N attempts you next drop should be from rare quality or better
* If you had already killed this monster unlock this kind of drop

---

## [ Drop with removal](#summary) 

Sometimes we want to remove an item from the collection after it has been dropped, this case shows how to work with instance tables and use the base table as a template only.

**Examples** 

* A box of cards, when you buy a pack, this pack is removed from the collection, so your chances to drop other things are changed
* A normal deck of card, when you draw a card, this card is removed from your deck that can be shuffled again using template, you can add and remove cards in runtime without change the base template (your original deck)

---

## [Repeatable drop](#summary)

Sometimes you want to be able to run the same sequence of drop
PS: Technically this isn't a feature from the system, because we use Unity random internally so this only show an example of how to control random seed

**Example**

* You're debugging your game
* Someone open the box and didn't liked what him got, so why not reopen game and try again 

---

## [Local and temporary modifiers](#summary)

How to inject a modifier base on a set of some local variable(s). In this example I'm showing how to apply local modifiers to a table and also how to ask for a drop passing an temporary modifier

**Examples**

* The angrier the monster is the better your chances will be
* This time only I want to get the double amount of items

---

## [Custom logic](#summary)

Sometimes we want more control about drops will happen, so in this example I try to show how to change the way that the drop will happen

---

## [Drop with global modifier](#summary)

Exactly like a local modifier, but this will happen in a global context, so every table will run it.

**Example** 

* After you find the Golden Grail every item in your game will have an increase chance of 20%
* After you eat THE Golden Apple every consumable in your game will double the drop chance
