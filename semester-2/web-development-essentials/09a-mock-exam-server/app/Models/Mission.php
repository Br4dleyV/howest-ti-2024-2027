<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Concerns\HasUuids;
use Illuminate\Database\Eloquent\Model;

class Mission extends Model
{
    use HasUuids;

    protected $fillable = [
        "destination_id",
        "email_captain",
        "report",
        "success",
    ];

    public function destination()
    {
        return $this->belongsTo(Destination::class);
    }
    public function astronauts()
    {
        return $this->belongsToMany(Astronaut::class);
    }
}