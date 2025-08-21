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
    hint: str = ""
    
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
    
    def to_jeopardy_format(self):
        """Convert to Unity .jeopardy format"""
        categories_data = []
        
        for category in self.categories:
            # Convert category to Unity format
            cat_data = {
                "categoryName": category.name,
                "categoryColorR": int(category.color.r * 255),
                "categoryColorG": int(category.color.g * 255), 
                "categoryColorB": int(category.color.b * 255),
                "categoryHint": category.hint,
                "questions": []
            }
            
            # Convert questions to Unity format
            for question in category.questions:
                q_data = {
                    "value": question.value,
                    "questionColorR": int(question.questionColor.r * 255),
                    "questionColorG": int(question.questionColor.g * 255),
                    "questionColorB": int(question.questionColor.b * 255),
                    "PresentationType": 0 if question.type == "text" else 1,  # 0=text, 1=media
                    "questionText": question.question,
                    "questionImage": question.questionMediaPath,
                    "questionVideo": None,  # Unity format has separate image/video fields
                    "answerImage": question.answerMediaPath,
                    "answerVideo": None,
                    "answer": question.answer,
                    "isAvailable": True,
                    "questionNote": question.note or ""
                }
                cat_data["questions"].append(q_data)
            
            categories_data.append(cat_data)
        
        return categories_data
    
    def save_to_jeopardy_file(self, filepath):
        """Save quiz to Unity .jeopardy file format"""
        categories_data = self.to_jeopardy_format()
        
        with open(filepath, 'w', encoding='utf-8') as f:
            # Line 1: Game name
            f.write(self.gameName + '\n')
            # Line 2: Tagline
            f.write(self.tagline + ' \n')
            # Line 3+: Categories JSON array
            json.dump(categories_data, f, separators=(',', ':'), ensure_ascii=False)
    
    @classmethod
    def from_jeopardy_format(cls, game_name, tagline, categories_data):
        """Create Quiz from Unity .jeopardy format data"""
        categories = []
        
        for cat_data in categories_data:
            # Convert Unity category format to internal format
            color = Color(
                r=cat_data.get("categoryColorR", 0) / 255.0,
                g=cat_data.get("categoryColorG", 0) / 255.0,
                b=cat_data.get("categoryColorB", 255) / 255.0,
                a=1.0
            )
            
            questions = []
            for q_data in cat_data.get("questions", []):
                # Convert Unity question format to internal format
                question_color = Color(
                    r=q_data.get("questionColorR", 0) / 255.0,
                    g=q_data.get("questionColorG", 0) / 255.0,
                    b=q_data.get("questionColorB", 255) / 255.0,
                    a=1.0
                )
                
                # Determine media path (Unity has separate image/video fields)
                question_media = q_data.get("questionImage") or q_data.get("questionVideo")
                answer_media = q_data.get("answerImage") or q_data.get("answerVideo")
                
                question = Question(
                    value=q_data.get("value", 100),
                    type="text" if q_data.get("PresentationType", 0) == 0 else "media",
                    question=q_data.get("questionText", ""),
                    answer=q_data.get("answer", ""),
                    questionMediaPath=question_media,
                    answerMediaPath=answer_media,
                    note=q_data.get("questionNote", ""),
                    questionColor=question_color
                )
                questions.append(question)
            
            category = Category(
                name=cat_data.get("categoryName", "Unnamed Category"),
                color=color,
                questions=questions,
                hint=cat_data.get("categoryHint", "")
            )
            categories.append(category)
        
        return cls(
            gameName=game_name,
            tagline=tagline,
            categories=categories
        )
    
    @classmethod
    def load_from_jeopardy_file(cls, filepath):
        """Load quiz from Unity .jeopardy file"""
        with open(filepath, 'r', encoding='utf-8') as f:
            lines = f.readlines()
        
        if len(lines) < 3:
            raise ValueError("Invalid .jeopardy file format: must have at least 3 lines")
        
        # Parse the 3-line format
        game_name = lines[0].strip()
        tagline = lines[1].strip()
        categories_json = ''.join(lines[2:]).strip()
        
        # Parse categories JSON
        categories_data = json.loads(categories_json)
        
        return cls.from_jeopardy_format(game_name, tagline, categories_data)
