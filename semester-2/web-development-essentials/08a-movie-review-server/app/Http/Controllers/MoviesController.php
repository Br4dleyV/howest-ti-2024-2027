<?php

namespace App\Http\Controllers;
use App\Models\Movie;
use App\Models\Review;
use Illuminate\Support\Facades\Log;
use Illuminate\Support\Facades\Validator;
use Illuminate\Http\Request;

class MoviesController extends Controller
{
    public function getMovies()
    {
        Log::info('Fetching all movies');
        return [ "data" => Movie::all() ];
    }

    public function getReviewsForMovie($movieId)
    {
        $movie = $this->findMovie($movieId);
        return response()->json([
            'data' => $movie->reviews()->get()
        ]);
    }

    public function newReviewForMovie(Request $request)
    {
        $movieId = $request->route('id');
        $review = $request->only(['email', 'message', 'rating']);
        $movie = $this->findMovie(movieId: $movieId);

        $validator = Validator::make($request->all(), [
            'email' => 'required|email|max:255',
            'message' => 'required|string|min:5|max:255',
            'rating' => 'required|integer|min:1|max:5',
        ]);

        if ($validator->fails()) {
            Log::warning('Validation failed for new review', [
                'errors' => $validator->errors(),
                'request' => $request->all()
            ]);
            return response()->json([
                'success' => false,
                'message' => $validator->errors(),
            ], 422);
        }

        $review = new Review($review);
        $review->movie()->associate($movie);
        $review->save();

        return response()->json([
            'success' => true,
            'message' => 'Review created successfully',
            'data' => $review
        ], 201);
    }

    private function findMovie($movieId)
    {
        $movie = Movie::find($movieId);
        if (!$movie) {
            return response()->json(['error' => 'Movie not found'], 404);
        }
        return $movie;
    }
}
