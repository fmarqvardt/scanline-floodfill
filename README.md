# scanline-floodfill
Attempt at making a fast floodfill algorithm utilizing full AVX2 256-bit registers, AVX2 bitwise operators and burst.
![](scanline.gif)

Above gif is a small demonstration how it executes. In reality it executes in 16x16 bit chunks.


Burst vectorization:

![](vectorized.PNG)

Dependencies:
- [MaxMath](https://www.github.com/MrUnbelievable92/MaxMath)
