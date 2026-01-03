#!/bin/bash

echo "=== Echoes of Elder Dawn - Quick Setup ==="
echo ""

# Check if Unity Hub is installed
if command -v unity-hub &> /dev/null; then
    echo "✅ Unity Hub found"
else
    echo "❌ Unity Hub not found. Please install Unity Hub first."
    echo "Download from: https://unity3d.com/get-unity/download"
    exit 1
fi

echo ""
echo "📁 Project files are ready in:"
echo "   $(pwd)"
echo ""
echo "🎯 Next steps:"
echo "1. Open Unity Hub"
echo "2. Click 'New Project'"
echo "3. Select '2D Template'"
echo "4. Set project name: 'Echoes of Elder Dawn'"
echo "5. Set location to: $(pwd)"
echo "6. Click 'Create Project'"
echo ""
echo "🔧 After Unity opens:"
echo "1. Import TextMeshPro (Window > TextMeshPro > Import TMP Essential Resources)"
echo "2. Create empty GameObject in scene"
echo "3. Add 'AutoSetup' script to it"
echo "4. Click 'Setup Complete Game' in inspector"
echo "5. Press Play to test!"
echo ""
echo "🎮 Game Controls:"
echo "   WASD/Arrows - Move"
echo "   Q - Quest Menu"
echo "   E - Interact"
echo "   Go to position (8,8) for main quest"
echo ""
echo "Ready to build your RPG! 🚀"