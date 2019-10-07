{

    "bones": [
        {
            "name": "ChestBottom",
            "color1": "Blue",
            "color2": "Blue",
            "frequency": 40
        },
        {
            "name": "LeftUpperArm",
            "color1": "Pink",
            "color2": "Pink",
            "frequency": 40
        },
        {
            "name": "LeftForeArm",
            "color1": "Yellow",
            "color2": "Yellow",
            "frequency": 40
        },
        {
            "name": "RightUpperArm",
            "color1": "Red",
            "color2": "Red",
            "frequency": 40
        },
        {
            "name": "RightForeArm",
            "color1": "Green",
            "color2": "Green",
            "frequency": 40
        },
        {
            "name": "Hip",
            "color1": "Cyan",
            "color2": "Cyan",
            "frequency": 40
        }
    ],
    "master_bone": "ChestBottom",
    "special": {
        "bone": "ChestBottom",
        "orientation": "Front"
    },
    "constraints": [
        {
            "type": "INTERP",
            "target": "Tummy",
            "source": "ChestBottom",
            "f": 0.5
        },
        {
            "type": "INTERP",
            "target": "ChestTop",
            "source": "Hip",
            "f": -0.42
        }
    ]
}
