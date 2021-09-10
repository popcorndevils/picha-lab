# Picha
Tool for generating sprites

# About
Hi, this is a tool I've developed on and off for a couple of years, you might have seen it in other forms.  Well, I don't see as many godot applications out there as examples so I've decided to open up the code to open source so you can see it, and maybe it will help you with your own projects.  
  
Caveat, feel free to fork your own projects off of this, I don't have time to review pull requests so even if you have a great idea I probably won't be able to incorporate it, you never know though.

If you're wondering why I split the code the way I have, it's because of a few things.  OctavianLib is where I put the general C# code extensions that I use across most of my projects.  PichaLib is the backbone of the procedural generation process.  I split it off from the application itself because my plan is to make a .dll that exposes some basic generation functions to use with .plab files.  That way you have two ways of using the assets from this application in your own games or whatever, you can export the sprites into sheets which you can ingest how you however you want, or you could load in .plab files and use the dynamic library to generate custom assets on the fly in your game.

This code is a mess though, I only ever work on it a bit at a time, I'm not consistent in my coding practices and I'm terrible about documentation.  This is basically just my crappy little project I use to occupy some free time.
