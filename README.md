# BlazorGalaga

A recreation of the classic Namco fixed shooter arcade game Galaga written in C# using the Blazor SPA framework and .NET Core. This is not an emulation or virtual machine, but a complete re-implementation of the game from the ground up.

[Demo Here](https://blazorguy.net/Blazor/BlazorGalaga/)

![BlazorGalaga](/BlazorGalaga/wwwroot/Assets/screenshot.PNG?raw=true "BlazorGalaga")

### Blazor Extensions Canvas

The graphics are rendered using the HTML Canvas element, leveraging the [Blazor Extensions Canvas](https://github.com/BlazorExtensions/Canvas) library. This allows all drawing and animation to be implemented in C#.

### Howler.Blazor

Sound logic is implemented using the [Howler.Blazor](https://github.com/StefH/Howler.Blazor) library.

### Mobile Support

Touch support for mobile is implemented with the [Hammer.js](https://hammerjs.github.io/) Javascript library. 

### Peformance

The game is optimized to run at 60FPS. There are cases where rendering 40+ sprites at once will decrease performance on slower machines and phones. The animation engine contains logic that automatically adjusts FPS based on system performance. 

### Future Enhancements

The game runs on a serverless architecture or CDN. Because of this, high score saving logic is nonexistent. High score saving can be easily added, but a server to store the high scores will be required. 

The game contains the first three challenge levels and infinite game levels. The original Galaga has eight total challenge levels and a maximum of 255 game levels. For the average player, these differences are unnoticeable as most players never make it past level 20.
