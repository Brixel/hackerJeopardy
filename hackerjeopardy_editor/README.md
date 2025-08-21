# Hacker Jeopardy Editor

A simple GUI editor for creating and editing Hacker Jeopardy quiz files. This replaces the clunky Unity editor with a user-friendly Python/Tkinter interface.

## Features

### Core Functionality
- **Load/Save JSON files** - Open existing quiz files or create new ones
- **Category Management** - Add, edit, and delete quiz categories with custom colors
- **Question Management** - Add, edit, and delete questions within categories
- **Simple Form UI** - Easy-to-use text fields and controls for all properties
- **Basic Validation** - Ensures required fields are filled and values are valid

### Advanced Features
- **Color Picker** - Visual color selection for categories and questions
- **Media File Browser** - Easy selection of question and answer media files
- **Question Type Support** - Text, image, audio, and video question types
- **Real-time Updates** - Changes are reflected immediately in the interface

## Requirements

- Python 3.8 or higher
- Tkinter (included with Python)
- No additional dependencies required!

## Installation

1. Clone or download the project files
2. Ensure Python 3.8+ is installed
3. Run the editor:

```bash
cd hackerjeopardy_editor
python3 main.py
```

## Usage

### Getting Started

1. **Create a New Quiz**: Use File → New to start with a blank quiz
2. **Load Sample Data**: Use File → Open and select `example_quiz.json` to see a working example
3. **Edit Quiz Info**: Update the game name and tagline at the top of the window

### Working with Categories

1. **Add Category**: Click "Add Category" in the Categories pane
2. **Edit Category**: Select a category from the list, then use the Category tab in the Editor pane
3. **Change Color**: Click "Choose Color" to pick a custom category color
4. **Delete Category**: Select a category and click "Delete Category"

### Working with Questions

1. **Add Question**: Select a category, then click "Add Question" in the Questions pane
2. **Edit Question**: Select a question from the list, then use the Question tab in the Editor pane
3. **Set Properties**: Fill in value, type, question text, answer, and optional note
4. **Add Media**: Use the Browse buttons to select question or answer media files
5. **Change Color**: Click "Choose Color" to pick a custom question color
6. **Delete Question**: Select a question and click "Delete Question"

### File Operations

- **New**: File → New (creates blank quiz)
- **Open**: File → Open (loads existing JSON file)
- **Save**: File → Save (saves to current file)
- **Save As**: File → Save As (saves to new file)

## JSON Format

The editor creates JSON files compatible with the Hacker Jeopardy Unity game. The format includes:

```json
{
  "gameName": "Quiz Title",
  "tagline": "Quiz Description",
  "categories": [
    {
      "name": "Category Name",
      "color": {"r": 1.0, "g": 0.0, "b": 0.0, "a": 1.0},
      "questions": [
        {
          "value": 100,
          "type": "text",
          "question": "Question text",
          "answer": "Answer text",
          "questionMediaPath": null,
          "answerMediaPath": null,
          "note": "Optional note",
          "questionColor": {"r": 0.8, "g": 0.2, "b": 0.2, "a": 1.0}
        }
      ]
    }
  ]
}
```

## Tips

- **Save Frequently**: Use Ctrl+S or File → Save to avoid losing work
- **Use the Sample**: Load `example_quiz.json` to see how a complete quiz is structured
- **Color Coding**: Use different colors for categories and questions to make them visually distinct
- **Media Paths**: Media file paths can be relative or absolute
- **Question Values**: Typically use 100, 200, 300, 400, 500 for Jeopardy-style scoring

## Troubleshooting

### Common Issues

1. **Application won't start**: Ensure Python 3.8+ is installed and Tkinter is available
2. **Can't load file**: Check that the JSON file is valid and follows the expected format
3. **Colors not showing**: Make sure color values are between 0.0 and 1.0
4. **Media files not found**: Use absolute paths or ensure relative paths are correct

### Error Messages

- **"Question text is required"**: Fill in the question field before saving
- **"Answer text is required"**: Fill in the answer field before saving
- **"Question value must be positive"**: Enter a number greater than 0 for the question value

## Development

The editor consists of three main files:

- `models.py`: Data classes for Quiz, Category, Question, and Color
- `main.py`: Main GUI application with Tkinter interface
- `example_quiz.json`: Sample quiz data for testing

The code is designed to be simple and maintainable, focusing on core functionality over advanced features.

## License

This project is part of the Hacker Jeopardy game system. Use and modify as needed for your quiz creation needs.
