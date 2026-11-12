<?php

use App\Http\Controllers\AuditController;
use App\Http\Middleware\ValidateToken;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\Route;

Route::get("/organizations", [AuditController::class, "getAllOrganizations"]);
Route::get("/accounts", [AuditController::class, "getAllAccounts"])->middleware(ValidateToken::class);
Route::post("/launch-audit", [AuditController::class, "launchAudit"]);