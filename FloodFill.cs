using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MaxMath;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;

public static class FloodFill
{
    public static ushort16 SIMD_FloodFill(ushort16 obstacleMap, ushort2 startPosition)
    {
        NativeReference<ushort16> nativeReference = new NativeReference<ushort16>(obstacleMap, Allocator.TempJob);
        
        var job = new SIMD_FloodFill() { _obstacleMap = nativeReference, startingPosition = startPosition};
        job.Schedule().Complete();

        obstacleMap = job._obstacleMap.Value;
        nativeReference.Dispose();

        return obstacleMap;
    }
    
    public static JobHandle SIMD_FloodFill_JobHandle(ushort16 obstacleMap, ushort2 startPosition)
    {
        NativeReference<ushort16> nativeReference = new NativeReference<ushort16>(obstacleMap, Allocator.TempJob);
        var job = new SIMD_FloodFill() { _obstacleMap = nativeReference, startingPosition = startPosition};
        
        return job.Schedule();
    }
}

[BurstCompile]
public struct SIMD_FloodFill : IJob
{
    public NativeReference<ushort16> _obstacleMap;
    public ushort2 startingPosition;
    
    public void Execute()
    {
        ushort16 fillMap = new ushort16();
        ushort16 mask =~ _obstacleMap.Value;

        fillMap[startingPosition.y] = (ushort)(1 << startingPosition.x);
        fillMap &= mask;
        
        ushort16 shiftUpDown;
        ushort16 temp;
        do
        {
            temp = fillMap;

            fillMap |= ((fillMap << 1) | (fillMap >> 1));
            fillMap &= mask;

            shiftUpDown = new ushort16(0, fillMap.x0, fillMap.x1, fillMap.x2,
                fillMap.x3, fillMap.x4, fillMap.x5, fillMap.x6, fillMap.x7,
                fillMap.x8, fillMap.x9, fillMap.x10, fillMap.x11, fillMap.x12,
                fillMap.x13, fillMap.x14);
            
            fillMap |= shiftUpDown;

            shiftUpDown = new ushort16(fillMap.x1, fillMap.x2,
                fillMap.x3, fillMap.x4, fillMap.x5, fillMap.x6, fillMap.x7,
                fillMap.x8, fillMap.x9, fillMap.x10, fillMap.x11, fillMap.x12,
                fillMap.x13, fillMap.x14, fillMap.x15, 0);
            
            fillMap |= shiftUpDown;
            fillMap &= mask;
            
        } while 
            (!maxmath.all(fillMap == temp));

        _obstacleMap.Value = fillMap;
    }
}