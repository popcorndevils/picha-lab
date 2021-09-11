# Picha
Tool for generating sprites  
<img src="https://raw.githubusercontent.com/popcorndevils/picha/master/riblet.gif" width="200" title="Riblet the Frog was generated using Picha Lab"/>

# About
Hi, this is a tool I've developed on and off for a couple of years, you might have seen it in other forms.  Well, I don't see as many godot applications out there as examples so I've decided to open up the code to open source so you can see it, and maybe it will help you with your own projects.  
  
Caveat, feel free to fork your own projects off of this, I don't have time to review pull requests so even if you have a great idea I probably won't be able to incorporate it, you never know though.

If you're wondering why I split the code the way I have, it's because of a few things.  OctavianLib is where I put the general C# code extensions that I use across most of my projects.  PichaLib is the backbone of the procedural generation process.  I split it off from the application itself because my plan is to make a .dll that exposes some basic generation functions to use with .plab files.  That way you have two ways of using the assets from this application in your own games or whatever, you can export the sprites into sheets which you can ingest how you however you want, or you could load in .plab files and use the dynamic library to generate custom assets on the fly in your game.

This code is a mess though, I only ever work on it a bit at a time, I'm not consistent in my coding practices and I'm terrible about documentation.  This is basically just my crappy little project I use to occupy some free time.

# LICENSE
Copyright 2021 PopcornDevils

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
