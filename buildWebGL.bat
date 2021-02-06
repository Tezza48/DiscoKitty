@echo off
echo "Building Webgl"
unity -batchmode -quit -projectpath . -buildtarget WebGl -executeMethod BuildScripts.BuildWebGL && echo "Completed Successfully"