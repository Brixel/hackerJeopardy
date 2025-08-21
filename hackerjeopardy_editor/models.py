import json
from dataclasses import dataclass, asdict
from typing import List, Optional

@dataclass
class Color:
    r: float = 0.0
    g: float = 0.0  
    b: float = 1.0
    a: float = 1.0
    
    def to_hex(self):
        """Convert to hex color for tkinter color picker"""
        r = int(self.r * 255)
        g = int(self.g * 255)
        b = int(self.b * 255)
        return f"#{r:02x}{g:02x}{b:02x}"
    
    @classmethod
    def from_hex(cls, hex_color):
        """Create Color from hex string"""
        hex_color = hex_color.lstrip('#')
        r = int(hex_color[0:2], 16) / 255.0
        g = int(hex_color[2:4], 16) / 255.0
        b = int(hex_color[4:6], 16) / 255.0
        return cls(r, g, b, 1.0)

@dataclass  
class Question:
    value: int = 100
    type: str = "text"
    question: str = ""
    answer: str = ""
    questionMediaPath: Optional[str] = None
    answerMediaPath: Optional[str] = None
    note: Optional[str] = ""
    questionColor: Color = None
    
    def __post_init__(self):
        if self.questionColor is None:
            self.questionColor = Color(0.2, 0.2, 0.8, 1.0)
    
    def validate(self):
        """Validate question data"""
        errors = []
        if not self.question.strip():
            errors.append("Question text is required")
        if not self.answer.strip():
            errors.append("Answer text is required")
        if self.value <= 0:
            errors.append("Question value must be positive")
        return errors

@dataclass
class Category:
    name: str = "New Category"
    color: Color = None
    questions: List[Question] = None
    
    def __post_init__(self):
        if self.color is None:
            self.color = Color(0.0, 0.5, 1.0, 1.0)
        if self.questions is None:
            self.questions = []
    
    def validate(self):
        """Validate category data"""
        errors = []
        if not self.name.strip():
            errors.append("Category name is required")
        return errors

@dataclass
class Quiz:
    gameName: str = "New Quiz"
    tagline: str = "Enter tagline here"
    categories: List[Category] = None
    
    def __post_init__(self):
        if self.categories is None:
            self.categories = []
    
    def validate(self):
        """Validate quiz data"""
        errors = []
        if not self.gameName.strip():
            errors.append("Game name is required")
        if not self.tagline.strip():
            errors.append("Tagline is required")
        return errors
    
    def to_dict(self):
        """Convert to dictionary for JSON serialization"""
        return asdict(self)
    
    @classmethod
    def from_dict(cls, data):
        """Create Quiz from dictionary (JSON data)"""
        # Convert color dictionaries back to Color objects
        categories = []
        for cat_data in data.get('categories', []):
            # Convert category color
            if 'color' in cat_data and cat_data['color']:
                cat_data['color'] = Color(**cat_data['color'])
            
            # Convert question colors
            questions = []
            for q_data in cat_data.get('questions', []):
                if 'questionColor' in q_data and q_data['questionColor']:
                    q_data['questionColor'] = Color(**q_data['questionColor'])
                questions.append(Question(**q_data))
            
            cat_data['questions'] = questions
            categories.append(Category(**cat_data))
        
        data['categories'] = categories
        return cls(**data)
    
    def save_to_file(self, filepath):
        """Save quiz to JSON file"""
        with open(filepath, 'w', encoding='utf-8') as f:
            json.dump(self.to_dict(), f, indent=2, ensure_ascii=False)
    
    @classmethod
    def load_from_file(cls, filepath):
        """Load quiz from JSON file"""
        with open(filepath, 'r', encoding='utf-8') as f:
            data = json.load(f)
        return cls.from_dict(data)
