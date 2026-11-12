<?php

use App\Http\Controllers\MoviesController;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\Route;

Route::get("/movies", [MoviesController::class, 'getMovies']);
Route::get("/movies/{id}/reviews", [MoviesController::class, 'getReviewsForMovie']);
Route::post("/movies/{id}/reviews", [MoviesController::class, "newReviewForMovie"]);