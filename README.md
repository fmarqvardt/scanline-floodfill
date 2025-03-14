# scanline-floodfill
Attempt at making a fast floodfill algorithm utilizing full AVX2 256-bit registers, AVX2 bitwise operators and burst.
![](scanline.gif)

Above gif is a demonstration how it executes. Real version is locked to 16x16 bit chunks.

![](vectorized.PNG)

Dependencies:
- [MaxMath](https://www.github.com/MrUnbelievable92/MaxMath)
