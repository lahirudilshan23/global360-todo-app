import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TodoService } from '../../services/todo.service';
import { Todo } from '../../models/todo.models';
import { ChangeDetectorRef } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { MatCardModule } from '@angular/material/card';
import { MatListModule } from '@angular/material/list';
import { MatIconModule } from '@angular/material/icon';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-todo',
  standalone: true,
  imports: [
    CommonModule, 
    FormsModule,
    MatButtonModule,
    MatInputModule,
    MatCardModule,
    MatListModule,
    MatIconModule,
    MatToolbarModule
  ],
  templateUrl: './todo.component.html',
  styleUrls: ['./todo.component.css']
})
export class TodoComponent {
  todos: Todo[] = [];
  title = '';
  editingTodoId: number | null = null;

  constructor(
    private todoService: TodoService, 
    private authService: AuthService,
    private router: Router,
    private cdr: ChangeDetectorRef,
    private snackBar: MatSnackBar
  ) 
  {
    this.loadTodos();
  }

  loadTodos() {
    this.todoService.getTodos().subscribe(t => {
      this.todos = t;
      this.cdr.detectChanges(); // Force Angular to update the view
    });
  }

  addTodo() {
    if (!this.title.trim()) return;

    this.todoService.addTodo(this.title).subscribe({
      next: () => {
        this.title = '';
        this.loadTodos();
      },
      error: err => console.error(err)
    });
  }

  logout() {
    this.authService.logout();
    this.router.navigate(['/']);
  }

  edit(todo: Todo) {
    this.title = todo.title;
    this.editingTodoId = todo.id;
  }

  saveTodo() {
    if (!this.title.trim()) return;

    // UPDATE
    if (this.editingTodoId !== null) {

      const todo: Todo = {
        id: this.editingTodoId,
        title: this.title
      };

      this.todoService.updateTodo(todo).subscribe(() => {
        this.showMessage('Todo updated successfully');
        this.resetForm();
        this.loadTodos();
      });
      return;
    }

    // ADD
    this.todoService.addTodo(this.title).subscribe(() => {
      this.showMessage('Todo added successfully');
      this.resetForm();
      this.loadTodos();
    });
  }

  deleteTodo(id: number): void {
    this.todoService.deleteTodo(id).subscribe(() => {
      this.showMessage('Todo deleted');
      this.loadTodos();
    });
  }

  resetForm() {
    this.title = '';
    this.editingTodoId = null;
  }

  private showMessage(message: string): void {
    this.snackBar.open(message, 'Close', {
      duration: 3000,
      horizontalPosition: 'center',
      verticalPosition: 'bottom'
    });
  }
}