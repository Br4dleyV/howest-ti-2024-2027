<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Concerns\HasUuids;
use Illuminate\Database\Eloquent\Model;

class Review extends Model
{
    use HasUuids;

    protected $fillable = [
        'email',
        'message',
        'rating',
    ];

    public function movie()
    {
        return $this->belongsTo(Movie::class);
    }
}
