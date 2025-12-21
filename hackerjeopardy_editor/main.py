import tkinter as tk
from tkinter import ttk, filedialog, messagebox, colorchooser
from models import Quiz, Category, Question, Color

class JeopardyEditor:
    def __init__(self):
        self.root = tk.Tk()
        self.root.title("Hacker Jeopardy Editor")
        self.root.geometry("1200x800")
        self.quiz = Quiz()
        self.current_category = None
        self.current_question = None
        self.current_file = None
        self.setup_ui()
        self.refresh_all()
    
    def setup_ui(self):
        # Menu bar
        menubar = tk.Menu(self.root)
        self.root.config(menu=menubar)
        
        file_menu = tk.Menu(menubar, tearoff=0)
        menubar.add_cascade(label="File", menu=file_menu)
        file_menu.add_command(label="New", command=self.new_quiz)
        file_menu.add_command(label="Open", command=self.open_quiz)
        file_menu.add_command(label="Save", command=self.save_quiz)
        file_menu.add_command(label="Save As", command=self.save_quiz_as)
        file_menu.add_separator()
        file_menu.add_command(label="Export as .jeopardy", command=self.export_jeopardy)
        file_menu.add_separator()
        file_menu.add_command(label="Exit", command=self.root.quit)
        
        # Main layout - 3 panes
        main_frame = ttk.Frame(self.root)
        main_frame.pack(fill=tk.BOTH, expand=True, padx=5, pady=5)
        
        # Quiz info frame at top
        quiz_frame = ttk.LabelFrame(main_frame, text="Quiz Information")
        quiz_frame.pack(fill=tk.X, pady=(0, 5))
        
        ttk.Label(quiz_frame, text="Game Name:").grid(row=0, column=0, sticky="w", padx=5, pady=2)
        self.game_name_var = tk.StringVar()
        self.game_name_entry = ttk.Entry(quiz_frame, textvariable=self.game_name_var, width=30)
        self.game_name_entry.grid(row=0, column=1, sticky="ew", padx=5, pady=2)
        self.game_name_var.trace('w', self.on_quiz_info_changed)
        
        ttk.Label(quiz_frame, text="Tagline:").grid(row=0, column=2, sticky="w", padx=5, pady=2)
        self.tagline_var = tk.StringVar()
        self.tagline_entry = ttk.Entry(quiz_frame, textvariable=self.tagline_var, width=40)
        self.tagline_entry.grid(row=0, column=3, sticky="ew", padx=5, pady=2)
        self.tagline_var.trace('w', self.on_quiz_info_changed)
        
        quiz_frame.columnconfigure(1, weight=1)
        quiz_frame.columnconfigure(3, weight=2)
        
        # Content frame for 2 panes (Matrix + Editor)
        content_frame = ttk.Frame(main_frame)
        content_frame.pack(fill=tk.BOTH, expand=True)
        
        # Matrix pane (replaces Categories and Questions)
        matrix_frame = ttk.LabelFrame(content_frame, text="Quiz Board")
        matrix_frame.grid(row=0, column=0, sticky="nsew", padx=(0, 5))
        
        # Matrix controls
        matrix_controls = ttk.Frame(matrix_frame)
        matrix_controls.pack(fill=tk.X, padx=5, pady=5)
        ttk.Button(matrix_controls, text="Add Category", command=self.add_category).pack(side=tk.LEFT, padx=2)
        ttk.Button(matrix_controls, text="Delete Category", command=self.delete_category).pack(side=tk.LEFT, padx=2)
        
        # Matrix grid container
        self.matrix_container = ttk.Frame(matrix_frame)
        self.matrix_container.pack(fill=tk.BOTH, expand=True, padx=5, pady=5)
        
        # Initialize matrix variables
        self.matrix_buttons = {}  # (cat_index, value) -> button
        self.selected_cell = None  # (cat_index, value)
        
        # Editor pane
        edit_frame = ttk.LabelFrame(content_frame, text="Editor")  
        edit_frame.grid(row=0, column=1, sticky="nsew", padx=(5, 0))
        
        # Create notebook for category and question editing
        self.notebook = ttk.Notebook(edit_frame)
        self.notebook.pack(fill=tk.BOTH, expand=True, padx=5, pady=5)
        
        # Category editor tab
        self.cat_edit_frame = ttk.Frame(self.notebook)
        self.notebook.add(self.cat_edit_frame, text="Category")
        self.setup_category_editor()
        
        # Question editor tab
        self.q_edit_frame = ttk.Frame(self.notebook)
        self.notebook.add(self.q_edit_frame, text="Question")
        self.setup_question_editor()
        
        # Configure grid weights (now 2 columns instead of 3)
        content_frame.columnconfigure(0, weight=1)
        content_frame.columnconfigure(1, weight=2)
        content_frame.rowconfigure(0, weight=1)
    
    def setup_category_editor(self):
        # Category name
        ttk.Label(self.cat_edit_frame, text="Category Name:").grid(row=0, column=0, sticky="w", padx=5, pady=5)
        self.cat_name_var = tk.StringVar()
        self.cat_name_entry = ttk.Entry(self.cat_edit_frame, textvariable=self.cat_name_var, width=30)
        self.cat_name_entry.grid(row=0, column=1, sticky="ew", padx=5, pady=5)
        self.cat_name_var.trace('w', self.on_category_changed)
        
        # Category color
        ttk.Label(self.cat_edit_frame, text="Color:").grid(row=1, column=0, sticky="w", padx=5, pady=5)
        color_frame = ttk.Frame(self.cat_edit_frame)
        color_frame.grid(row=1, column=1, sticky="ew", padx=5, pady=5)
        
        self.cat_color_canvas = tk.Canvas(color_frame, width=50, height=25, bg="blue")
        self.cat_color_canvas.pack(side=tk.LEFT, padx=(0, 5))
        ttk.Button(color_frame, text="Choose Color", command=self.choose_category_color).pack(side=tk.LEFT)
        
        # Category hint
        ttk.Label(self.cat_edit_frame, text="Hint:").grid(row=2, column=0, sticky="w", padx=5, pady=5)
        self.cat_hint_var = tk.StringVar()
        self.cat_hint_entry = ttk.Entry(self.cat_edit_frame, textvariable=self.cat_hint_var, width=30)
        self.cat_hint_entry.grid(row=2, column=1, sticky="ew", padx=5, pady=5)
        self.cat_hint_var.trace('w', self.on_category_changed)
        
        self.cat_edit_frame.columnconfigure(1, weight=1)
    
    def setup_question_editor(self):
        row = 0
        
        # Question value
        ttk.Label(self.q_edit_frame, text="Value:").grid(row=row, column=0, sticky="w", padx=5, pady=2)
        self.q_value_var = tk.StringVar()
        self.q_value_entry = ttk.Entry(self.q_edit_frame, textvariable=self.q_value_var, width=10)
        self.q_value_entry.grid(row=row, column=1, sticky="w", padx=5, pady=2)
        self.q_value_var.trace('w', self.on_question_changed)
        row += 1
        
        # Question type
        ttk.Label(self.q_edit_frame, text="Type:").grid(row=row, column=0, sticky="w", padx=5, pady=2)
        self.q_type_var = tk.StringVar()
        self.q_type_combo = ttk.Combobox(self.q_edit_frame, textvariable=self.q_type_var, 
                                        values=["text", "image", "audio", "video"], width=15)
        self.q_type_combo.grid(row=row, column=1, sticky="w", padx=5, pady=2)
        self.q_type_var.trace('w', self.on_question_changed)
        row += 1
        
        # Question text
        ttk.Label(self.q_edit_frame, text="Question:").grid(row=row, column=0, sticky="nw", padx=5, pady=2)
        self.q_question_text = tk.Text(self.q_edit_frame, height=4, width=40)
        self.q_question_text.grid(row=row, column=1, sticky="ew", padx=5, pady=2)
        self.q_question_text.bind('<KeyRelease>', self.on_question_text_changed)
        row += 1
        
        # Answer text
        ttk.Label(self.q_edit_frame, text="Answer:").grid(row=row, column=0, sticky="nw", padx=5, pady=2)
        self.q_answer_text = tk.Text(self.q_edit_frame, height=4, width=40)
        self.q_answer_text.grid(row=row, column=1, sticky="ew", padx=5, pady=2)
        self.q_answer_text.bind('<KeyRelease>', self.on_answer_text_changed)
        row += 1
        
        # Note
        ttk.Label(self.q_edit_frame, text="Note:").grid(row=row, column=0, sticky="nw", padx=5, pady=2)
        self.q_note_text = tk.Text(self.q_edit_frame, height=3, width=40)
        self.q_note_text.grid(row=row, column=1, sticky="ew", padx=5, pady=2)
        self.q_note_text.bind('<KeyRelease>', self.on_note_text_changed)
        row += 1
        
        # Question media path
        ttk.Label(self.q_edit_frame, text="Question Media:").grid(row=row, column=0, sticky="w", padx=5, pady=2)
        media_frame = ttk.Frame(self.q_edit_frame)
        media_frame.grid(row=row, column=1, sticky="ew", padx=5, pady=2)
        self.q_media_var = tk.StringVar()
        self.q_media_entry = ttk.Entry(media_frame, textvariable=self.q_media_var)
        self.q_media_entry.pack(side=tk.LEFT, fill=tk.X, expand=True, padx=(0, 5))
        ttk.Button(media_frame, text="Browse", command=self.browse_question_media).pack(side=tk.RIGHT)
        self.q_media_var.trace('w', self.on_question_changed)
        row += 1
        
        # Answer media path
        ttk.Label(self.q_edit_frame, text="Answer Media:").grid(row=row, column=0, sticky="w", padx=5, pady=2)
        answer_media_frame = ttk.Frame(self.q_edit_frame)
        answer_media_frame.grid(row=row, column=1, sticky="ew", padx=5, pady=2)
        self.q_answer_media_var = tk.StringVar()
        self.q_answer_media_entry = ttk.Entry(answer_media_frame, textvariable=self.q_answer_media_var)
        self.q_answer_media_entry.pack(side=tk.LEFT, fill=tk.X, expand=True, padx=(0, 5))
        ttk.Button(answer_media_frame, text="Browse", command=self.browse_answer_media).pack(side=tk.RIGHT)
        self.q_answer_media_var.trace('w', self.on_question_changed)
        row += 1
        
        # Question color
        ttk.Label(self.q_edit_frame, text="Question Color:").grid(row=row, column=0, sticky="w", padx=5, pady=2)
        q_color_frame = ttk.Frame(self.q_edit_frame)
        q_color_frame.grid(row=row, column=1, sticky="ew", padx=5, pady=2)
        
        self.q_color_canvas = tk.Canvas(q_color_frame, width=50, height=25, bg="blue")
        self.q_color_canvas.pack(side=tk.LEFT, padx=(0, 5))
        ttk.Button(q_color_frame, text="Choose Color", command=self.choose_question_color).pack(side=tk.LEFT)
        
        self.q_edit_frame.columnconfigure(1, weight=1)
    
    # File operations
    def new_quiz(self):
        if messagebox.askyesno("New Quiz", "Create a new quiz? Unsaved changes will be lost."):
            self.quiz = Quiz()
            self.current_category = None
            self.current_question = None
            self.current_file = None
            self.refresh_all()
    
    def open_quiz(self):
        filepath = filedialog.askopenfilename(
            title="Open Quiz File",
            filetypes=[("JSON files", "*.json"), ("Jeopardy files", "*.jeopardy"), ("All files", "*.*")]
        )
        if filepath:
            try:
                if filepath.lower().endswith('.jeopardy'):
                    self.quiz = Quiz.load_from_jeopardy_file(filepath)
                else:
                    self.quiz = Quiz.load_from_file(filepath)
                self.current_file = filepath
                self.current_category = None
                self.current_question = None
                self.refresh_all()
                messagebox.showinfo("Success", "Quiz loaded successfully!")
            except Exception as e:
                messagebox.showerror("Error", f"Failed to load quiz: {str(e)}")
    
    def save_quiz(self):
        if self.current_file:
            try:
                self.quiz.save_to_file(self.current_file)
                messagebox.showinfo("Success", "Quiz saved successfully!")
            except Exception as e:
                messagebox.showerror("Error", f"Failed to save quiz: {str(e)}")
        else:
            self.save_quiz_as()
    
    def save_quiz_as(self):
        filepath = filedialog.asksaveasfilename(
            title="Save Quiz As",
            defaultextension=".json",
            filetypes=[("JSON files", "*.json"), ("All files", "*.*")]
        )
        if filepath:
            try:
                self.quiz.save_to_file(filepath)
                self.current_file = filepath
                messagebox.showinfo("Success", "Quiz saved successfully!")
            except Exception as e:
                messagebox.showerror("Error", f"Failed to save quiz: {str(e)}")
    
    def export_jeopardy(self):
        filepath = filedialog.asksaveasfilename(
            title="Export as .jeopardy",
            defaultextension=".jeopardy",
            filetypes=[("Jeopardy files", "*.jeopardy"), ("All files", "*.*")]
        )
        if filepath:
            try:
                self.quiz.save_to_jeopardy_file(filepath)
                messagebox.showinfo("Success", "Quiz exported as .jeopardy successfully!")
            except Exception as e:
                messagebox.showerror("Error", f"Failed to export quiz: {str(e)}")
    
    # Category operations
    def add_category(self):
        category = Category()
        self.quiz.categories.append(category)
        self.create_matrix()
        # Select the new category
        self.select_category(len(self.quiz.categories) - 1)
    
    def delete_category(self):
        if self.current_category:
            cat_index = self.quiz.categories.index(self.current_category)
            if messagebox.askyesno("Delete Category", "Are you sure you want to delete this category?"):
                del self.quiz.categories[cat_index]
                self.current_category = None
                self.current_question = None
                self.selected_cell = None
                self.refresh_all()
    
    # Question operations (now handled through matrix clicks)
    def delete_question(self):
        if self.current_question and self.current_category:
            if messagebox.askyesno("Delete Question", "Are you sure you want to delete this question?"):
                self.current_category.questions.remove(self.current_question)
                self.current_question = None
                self.selected_cell = None
                self.create_matrix()
                self.refresh_question_editor()
    
    # Event handlers
    def on_quiz_info_changed(self, *args):
        self.quiz.gameName = self.game_name_var.get()
        self.quiz.tagline = self.tagline_var.get()
    
    
    def on_category_changed(self, *args):
        if self.current_category:
            self.current_category.name = self.cat_name_var.get()
            self.current_category.hint = self.cat_hint_var.get()
            self.create_matrix()  # Refresh matrix to show new category name
    
    def on_question_changed(self, *args):
        if self.current_question:
            try:
                self.current_question.value = int(self.q_value_var.get() or "0")
            except ValueError:
                pass
            self.current_question.type = self.q_type_var.get()
            self.current_question.questionMediaPath = self.q_media_var.get() or None
            self.current_question.answerMediaPath = self.q_answer_media_var.get() or None
            self.create_matrix()  # Refresh matrix to show changes
    
    def on_question_text_changed(self, event):
        if self.current_question:
            self.current_question.question = self.q_question_text.get("1.0", tk.END).strip()
            self.create_matrix()  # Refresh matrix to show new question text
    
    def on_answer_text_changed(self, event):
        if self.current_question:
            self.current_question.answer = self.q_answer_text.get("1.0", tk.END).strip()
    
    def on_note_text_changed(self, event):
        if self.current_question:
            self.current_question.note = self.q_note_text.get("1.0", tk.END).strip()
    
    # Color choosers
    def choose_category_color(self):
        if self.current_category:
            color = colorchooser.askcolor(color=self.current_category.color.to_hex())
            if color[1]:  # color[1] is the hex string
                self.current_category.color = Color.from_hex(color[1])
                self.refresh_category_editor()
    
    def choose_question_color(self):
        if self.current_question:
            color = colorchooser.askcolor(color=self.current_question.questionColor.to_hex())
            if color[1]:  # color[1] is the hex string
                self.current_question.questionColor = Color.from_hex(color[1])
                self.refresh_question_editor()
    
    # File browsers
    def browse_question_media(self):
        filepath = filedialog.askopenfilename(
            title="Select Question Media",
            filetypes=[("All files", "*.*"), ("Images", "*.png *.jpg *.jpeg *.gif"), 
                      ("Audio", "*.mp3 *.wav"), ("Video", "*.mp4 *.avi")]
        )
        if filepath:
            self.q_media_var.set(filepath)
    
    def browse_answer_media(self):
        filepath = filedialog.askopenfilename(
            title="Select Answer Media",
            filetypes=[("All files", "*.*"), ("Images", "*.png *.jpg *.jpeg *.gif"), 
                      ("Audio", "*.mp3 *.wav"), ("Video", "*.mp4 *.avi")]
        )
        if filepath:
            self.q_answer_media_var.set(filepath)
    
    # Matrix methods
    def create_matrix(self):
        """Create the Jeopardy-style matrix grid"""
        # Clear existing matrix
        for widget in self.matrix_container.winfo_children():
            widget.destroy()
        self.matrix_buttons.clear()
        
        if not self.quiz.categories:
            # Show message when no categories
            no_cat_label = ttk.Label(self.matrix_container, text="Add categories to see the quiz board", 
                                   font=("Arial", 12), foreground="gray")
            no_cat_label.pack(expand=True)
            return
        
        # Create grid frame
        grid_frame = ttk.Frame(self.matrix_container)
        grid_frame.pack(fill=tk.BOTH, expand=True)
        
        # Standard Jeopardy values
        values = [100, 200, 300, 400, 500]
        
        # Create header row with category names
        ttk.Label(grid_frame, text="", width=8).grid(row=0, column=0, padx=1, pady=1)  # Empty corner
        for col, category in enumerate(self.quiz.categories):
            header_btn = tk.Button(grid_frame, text=category.name, 
                                 bg=category.color.to_hex(), fg="white",
                                 font=("Arial", 10, "bold"), width=15, height=2,
                                 command=lambda c=col: self.select_category(c))
            header_btn.grid(row=0, column=col+1, padx=1, pady=1, sticky="ew")
        
        # Create value rows
        for row, value in enumerate(values):
            # Value label
            ttk.Label(grid_frame, text=f"${value}", font=("Arial", 10, "bold"), 
                     width=8).grid(row=row+1, column=0, padx=1, pady=1)
            
            # Question cells
            for col, category in enumerate(self.quiz.categories):
                question = self.find_question_by_value(category, value)
                cell_key = (col, value)
                
                if question:
                    # Existing question - show truncated text
                    text = question.question[:20] + "..." if len(question.question) > 20 else question.question
                    bg_color = self.lighten_color(category.color.to_hex())
                    btn = tk.Button(grid_frame, text=text, bg=bg_color, 
                                  font=("Arial", 9), width=15, height=3, wraplength=100,
                                  command=lambda c=col, v=value: self.select_cell(c, v))
                else:
                    # Empty cell - show add option
                    btn = tk.Button(grid_frame, text="+ Add\nQuestion", bg="#f0f0f0", 
                                  font=("Arial", 9), width=15, height=3, fg="gray",
                                  command=lambda c=col, v=value: self.select_cell(c, v))
                
                btn.grid(row=row+1, column=col+1, padx=1, pady=1, sticky="ew")
                self.matrix_buttons[cell_key] = btn
        
        # Configure column weights for resizing
        for col in range(len(self.quiz.categories) + 1):
            grid_frame.columnconfigure(col, weight=1)
    
    def find_question_by_value(self, category, value):
        """Find a question in a category by its value"""
        for question in category.questions:
            if question.value == value:
                return question
        return None
    
    def lighten_color(self, hex_color):
        """Lighten a hex color for better readability"""
        # Remove # if present
        hex_color = hex_color.lstrip('#')
        # Convert to RGB
        r = int(hex_color[0:2], 16)
        g = int(hex_color[2:4], 16)
        b = int(hex_color[4:6], 16)
        # Lighten by mixing with white
        r = min(255, r + 80)
        g = min(255, g + 80)
        b = min(255, b + 80)
        return f"#{r:02x}{g:02x}{b:02x}"
    
    def select_category(self, cat_index):
        """Select a category for editing"""
        if cat_index < len(self.quiz.categories):
            self.current_category = self.quiz.categories[cat_index]
            self.current_question = None
            self.selected_cell = None
            self.refresh_category_editor()
            self.refresh_question_editor()
            self.notebook.select(0)  # Switch to category tab
            self.highlight_selection()
    
    def select_cell(self, cat_index, value):
        """Select a cell (question) for editing"""
        if cat_index < len(self.quiz.categories):
            category = self.quiz.categories[cat_index]
            question = self.find_question_by_value(category, value)
            
            self.current_category = category
            self.selected_cell = (cat_index, value)
            
            if question:
                # Edit existing question
                self.current_question = question
            else:
                # Create new question
                question = Question(value=value)
                category.questions.append(question)
                self.current_question = question
                self.create_matrix()  # Refresh matrix to show new question
            
            self.refresh_category_editor()
            self.refresh_question_editor()
            self.notebook.select(1)  # Switch to question tab
            self.highlight_selection()
    
    def highlight_selection(self):
        """Highlight the selected cell in the matrix"""
        # Reset all button styles
        for (col, value), btn in self.matrix_buttons.items():
            category = self.quiz.categories[col] if col < len(self.quiz.categories) else None
            if category:
                question = self.find_question_by_value(category, value)
                if question:
                    btn.config(relief="raised", borderwidth=1)
                else:
                    btn.config(relief="raised", borderwidth=1)
        
        # Highlight selected cell
        if self.selected_cell and self.selected_cell in self.matrix_buttons:
            self.matrix_buttons[self.selected_cell].config(relief="solid", borderwidth=3)
    
    # Refresh methods
    def refresh_all(self):
        self.refresh_quiz_info()
        self.create_matrix()
        self.refresh_category_editor()
        self.refresh_question_editor()
    
    def refresh_quiz_info(self):
        self.game_name_var.set(self.quiz.gameName)
        self.tagline_var.set(self.quiz.tagline)
    
    def refresh_categories(self):
        self.category_listbox.delete(0, tk.END)
        for category in self.quiz.categories:
            self.category_listbox.insert(tk.END, category.name)
    
    def refresh_questions(self):
        self.question_listbox.delete(0, tk.END)
        if self.current_category:
            for question in self.current_category.questions:
                display_text = f"${question.value}: {question.question[:50]}..."
                self.question_listbox.insert(tk.END, display_text)
    
    def refresh_category_editor(self):
        if self.current_category:
            self.cat_name_var.set(self.current_category.name)
            self.cat_hint_var.set(self.current_category.hint)
            self.cat_color_canvas.config(bg=self.current_category.color.to_hex())
        else:
            self.cat_name_var.set("")
            self.cat_hint_var.set("")
            self.cat_color_canvas.config(bg="white")
    
    def refresh_question_editor(self):
        if self.current_question:
            self.q_value_var.set(str(self.current_question.value))
            self.q_type_var.set(self.current_question.type)
            
            self.q_question_text.delete("1.0", tk.END)
            self.q_question_text.insert("1.0", self.current_question.question)
            
            self.q_answer_text.delete("1.0", tk.END)
            self.q_answer_text.insert("1.0", self.current_question.answer)
            
            self.q_note_text.delete("1.0", tk.END)
            self.q_note_text.insert("1.0", self.current_question.note or "")
            
            self.q_media_var.set(self.current_question.questionMediaPath or "")
            self.q_answer_media_var.set(self.current_question.answerMediaPath or "")
            
            self.q_color_canvas.config(bg=self.current_question.questionColor.to_hex())
        else:
            self.q_value_var.set("")
            self.q_type_var.set("text")
            self.q_question_text.delete("1.0", tk.END)
            self.q_answer_text.delete("1.0", tk.END)
            self.q_note_text.delete("1.0", tk.END)
            self.q_media_var.set("")
            self.q_answer_media_var.set("")
            self.q_color_canvas.config(bg="white")
    
    def run(self):
        self.root.mainloop()

if __name__ == "__main__":
    app = JeopardyEditor()
    app.run()
