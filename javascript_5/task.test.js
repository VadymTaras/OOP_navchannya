import { describe, test, beforeEach } from "node:test";
import assert from "node:assert";
import TaskManager from "./TaskManager.js";

describe("Extended Task Management System Unit Tests", () => {
    let manager;

    beforeEach(() => {
        manager = new TaskManager();
    });

    test("Should successfully create a task", () => {
        const task = manager.createTask({
            id: "1",
            title: "Підготувати звіт",
            description: "Звіт по лабораторній роботі",
            status: "pending",
            priority: "high",
            dueDate: "2026-06-01",
            tags: ["навчання"]
        });

        assert.strictEqual(manager.tasks.length, 1);
        assert.strictEqual(task.title, "Підготувати звіт");
    });

    test("Should find task by id and return null if not found", () => {
        manager.createTask({ id: "10", title: "Task 10", status: "pending", dueDate: "2026-06-01" });

        const foundTask = manager.getTaskById("10");
        const missingTask = manager.getTaskById("999");

        assert.notStrictEqual(foundTask, null);
        assert.strictEqual(foundTask.title, "Task 10");
        assert.strictEqual(missingTask, null);
    });

    test("Should correctly handle updateTask and toggleTaskStatus", () => {
        const task = manager.createTask({
            id: "2",
            title: "Тестова задача",
            status: "pending",
            dueDate: "2026-06-01"
        });

        manager.updateTask("2", { title: "Оновлена назва", priority: "low" });
        assert.strictEqual(task.title, "Оновлена назва");
        assert.strictEqual(task.priority, "low");

        manager.toggleTaskStatus("2");
        assert.strictEqual(task.status, "completed");

        manager.toggleTaskStatus("2");
        assert.strictEqual(task.status, "pending");
    });

    test("Should delete individual task and handle missing IDs", () => {
        manager.createTask({ id: "3", title: "Task 3", status: "pending", dueDate: "2026-06-01" });

        const successDelete = manager.deleteTask("3");
        const failDelete = manager.deleteTask("999");

        assert.strictEqual(successDelete, true);
        assert.strictEqual(failDelete, false);
        assert.strictEqual(manager.tasks.length, 0);
    });

    test("Should delete multiple tasks by status and clear all repository", () => {
        manager.createTask({ id: "4", title: "T4", status: "completed", dueDate: "2026-06-01" });
        manager.createTask({ id: "5", title: "T5", status: "completed", dueDate: "2026-06-01" });
        manager.createTask({ id: "6", title: "T6", status: "pending", dueDate: "2026-06-01" });

        manager.deleteTasksByStatus("completed");
        assert.strictEqual(manager.tasks.length, 1);
        assert.strictEqual(manager.tasks[0].id, "6");

        manager.clearAllTasks();
        assert.strictEqual(manager.tasks.length, 0);
    });

    test("Should add and remove tags dynamically", () => {
        const task = manager.createTask({
            id: "7",
            title: "Задача з тегами",
            status: "pending",
            dueDate: "2026-06-01",
            tags: ["work"]
        });

        task.addTag("urgent");
        assert.ok(task.tags.includes("urgent"));

        task.removeTag("work");
        assert.ok(!task.tags.includes("work"));
    });

    test("Should correctly detect overdue status", () => {
        const pastTask = manager.createTask({
            id: "8",
            title: "Минула задача",
            status: "pending",
            dueDate: "2020-01-01"
        });

        assert.strictEqual(pastTask.isOverdue, true);
    });

    test("Should calculate accurate statistics and rates", () => {
        manager.createTask({ id: "9", title: "T9", status: "completed", dueDate: "2026-06-01" });
        manager.createTask({ id: "10", title: "T10", status: "pending", dueDate: "2026-06-01" });

        const stats = manager.getStats();
        assert.strictEqual(stats.total, 2);
        assert.strictEqual(stats.completed, 1);
        assert.strictEqual(stats.pending, 1);
        assert.strictEqual(manager.getCompletionRate(), "50%");
    });
});