#unturned-Tsunami

*(protip: class names are in bold and theres a tree at the bottom)*

This is a custom cheat built for unturned based off Pirate Perfection and MSZ Reborn.
I originally played with these cheats back in early 2016 and it was one of my main
influences to begin programming. I revisited the idea to make this cheat over the 
summer of 2017, and ended up with this. Lots of puns from my username included. If you
plan on using any of these methods or classes in your cheat itd be nice if you gave me 
a thanks in the credits for your cheat! Im making this open source because I believe that
everyone should have a way to see how these cheats work, and can have an idea of how to 
do it themselves, but it'd be nice if you didnt take my entire cheat and call it yours,
because it isnt! Use a method or two (I know I did in the development of this) and make
YOUR cheat, dont compile mine!

If you are someone trying to learn how to program or just want to make some cheats, I really
reccommend you simply learn a language and do it yourself, its not too complicated and I 
wrote this with less than a years experience in explicitly c#. Im only just now approaching a
year after development is in essense over. www.edx.org is a good place to learn if you want to
get into it, and id put in a link to an actual course but they're always closing and opening new
ones so thatd be redundant. Just search c#.

Anyway I digress. Tsunami hack is WAY over engineered and there are some classes that arent even
being used anymore! Some of which are:

- Original **Visuals** and **Aim** menus.
- **Quake** is *pretty* useless tbch it could have just been put into **WaveMaker**
- the *types* are pretty verbose, I could have condensed them some more
- **DB** is useless now as ive stopped using my database
- **DataColector** is also useless as the database isnt in use
- **Identifier**,**IMenuParent**,**ILibParent** arent in use anymore as they were too restrictive to me

The basis of Tsunami hack is as follows, 
1. ***Cast*** is called in **Hook** from one of the games existing dlls.
2. **Quake** calls **Loader** to load in the files on the system and download lists from my pastebin, 
   or in the past it connected to my database.
3. **Quake** calls **WaveMaker**'s ***start*** which sets up the blocker, and a couple other vars.
4. **Quake** calls **WaveMaker**'s ***OnUpdate*** which adds all of the game's objects.
5. All of the game objects added run their ***Start***,***OnUpdate***, and ***OnGUI*** methods when called
   by unity like in a normal unity game.
6. Lib methods for some of the hacks are called from the gameobjects in their methods, ie **VisualsV2**.***Start***
   is called from the menu **Visuals**.***Start***.

Most of the methods and classes arent documented at all (I hate having to document code), I just know what everthing does
already, but most of the method names are verbose enough to get an idea of whats going on.



Tree:

![Tree](https://i.imgur.com/ALOeoAk.png)

(Files with ** are either depreciated, or no longer used)

Other than that, im not going to tell you how to compile this if you dont already know, im not going to
tell you what references you need. If you dont already know what you're doing and you're attempting to 
call this your own, just leave now! I made this as a project to further my own skills, and I reccommend 
you do the same!

I might add a short description of each of the (main) classes in the future who knows.

Screenshots!

![Main Menu](https://i.imgur.com/HMwTWAN.png "Main menu of V1.0")
![Visuals Menu](https://i.imgur.com/iYJdjFh.png "Visuals menu of V1.0")
![Visuals Cont](http://i.imgur.com/K7gtzl9.png "Visuals Menu of V1.0")
![Aim Menu](http://i.imgur.com/AsfsgzV.png "Aim Menu of V1.0")
![Keybinds Menu](http://i.imgur.com/fNtIZue.png "Keybinds Menu of V1.0")
![Gameplay example](http://i.imgur.com/ybpk6hl.png "Gameplay using V1.0")
![Gameplay example](http://i.imgur.com/7wSonVO.png "Gameplay using V1.0")

In conclusion, I hope that this helps out some dev out there looking to get into programming or creating cheats, and 
it has been awfully fun developing this and interacting with everyone! ***I might create a private cheat in the
future, but no promises. Id like to focus on other languages now that this project is over. Cheers!***

If youve got a question about any of the classes, just let me know on discord or steam...
www.steamcommunity.com/id/tidall
https://discord.gg/QhakXeK
... Ill try to help!
