<?php

namespace App\Http\Controllers;

use App\Mail\MissionReportMail;
use App\Models\Astronaut;
use App\Models\Destination;
use App\Models\Mission;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\Validator;
use Illuminate\Support\Facades\Mail;
use Illuminate\Support\Facades\Log;

class MissionController extends Controller
{
    public function executeMission(Request $request)
    {
        $astronautUuids = $request->input('astronauts');
        $destinationUuid = $request->input('destination');
        $emailCaptain = $request->input('emailCaptain');

        // Validate the request
        $validator = Validator::make($request->all(), $this->getNewMissionRules());
        if ($validator->fails()) {
            return response()->json([
                'success' => false,
                'message' => $validator->errors(),
            ], 422);
        }

        // Find all astronauts with corresponding UUIDs
        $astronauts = Astronaut::whereIn('id', $astronautUuids)->get();
        $destination = Destination::where('id', $destinationUuid)->first();

        // Calculate total strength of all astronauts
        $totalAstronautEndurance = $astronauts->sum('endurance') * 1.5;
        $totalAstronautSkill = $astronauts->sum('skill');
        $totalAstronautStrength = $totalAstronautEndurance + $totalAstronautSkill;

        // Check if the total strength is enough for the mission
        $necessaryStrength = $destination->distance;        
        $success = $totalAstronautStrength >= $necessaryStrength;

        // Save mission details
        $mission = new Mission();
        $mission->success = $success;
        $mission->destination_id = $destination->id;
        $mission->email_captain = $emailCaptain;

        if ($success) {
            $mission->report = 'Mission to ' . $destination->name . ' was a success, thanks to the power of the astronauts ' . implode(', ', $astronauts->pluck('name')->toArray()) . '.';
        } else {
            $mission->report = 'Mission to ' . $destination->name . ' was a failure, due to poor performance of the astronauts ' . implode(', ', $astronauts->pluck('name')->toArray()) . '.';
        }

        // Save mission details
        $mission->save();
        Log::info('Mission executed', [
            'mission_id' => $mission->id,
            'success' => $success,
            'destination' => $destination->name,
            'astronauts' => $astronauts->pluck('name')->toArray(),
        ]);

        $mail = new MissionReportMail($mission);
        Mail::to("bradley.velle@student.howest.be")->send($mail);

        
        // Return data
        return response()->json([
            'success'=> $success,
            'report' => $mission->report,
        ]);
    }

    private function getNewMissionRules()
    {
        return [
            'astronauts' => 'required|array',
            'destination' => 'required|exists:destinations,id',
            'emailCaptain' => 'required|email|max:255',
        ];
    }
}
