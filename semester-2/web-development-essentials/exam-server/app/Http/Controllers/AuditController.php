<?php

namespace App\Http\Controllers;

use App\Mail\AuditReport;
use App\Models\Account;
use App\Models\Audit;
use App\Models\Organization;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\Mail;
use Illuminate\Support\Facades\Validator;
use Symfony\Component\HttpFoundation\Response;

class AuditController extends Controller
{
    public function getAllOrganizations() {
        return ["data" => Organization::all()];
    }

    public function getAllAccounts() {
        return ["data" => Account::all()];
    }

    public function launchAudit(Request $request) {
        // Validate the request
        $validator = Validator::make($request->all(), [
            'emailAuditor' => 'required|email|max:255',
            'organizationId' => 'required|exists:organizations,id',
            'accountIds' => 'required|array',
        ]);

        // If validation fails, return error response
        if ($validator->fails()) {
            return response()->json([
                'success' => false,
                'message' => $validator->errors(),
            ], Response::HTTP_UNPROCESSABLE_ENTITY);
        }

        // Extract data from request
        $emailAuditor = $request->input('emailAuditor');
        $organizationId = $request->input('organizationId');
        $accountIds = $request->input('accountIds');

        // Get the organization and accounts
        $organization = Organization::find($organizationId);
        $accounts = Account::whereIn('id', $accountIds)->get();

        // Initialize the audit results
        $auditResults = [];
        foreach ($accounts as $account) {
            $passwordStrength = $this->calculatePasswordStrength($account->password);
            $auditResults[] = [
                'accountId' => $account->id, // Account ID
                'name' => $account->name, // Account name
                'passwordStrength' => $passwordStrength, // Password strength score
                'success' => $passwordStrength >= 12, // Success if strength is 12 or more, otherwisee false
            ];
        }

        $report = $this->buildReport($auditResults, $organization);

        $audit = new Audit();
        $audit->organization_id = $organizationId;
        $audit->email = $emailAuditor;
        $audit->success = $report[0]; // true if audit succeeded, false otherwise
        $audit->report = $report[1];
        $audit->save();

        // Send email to the captain with the mission report
        $mail = new AuditReport($audit);
        //Mail::to($emailAuditor)->send($mail);

        return response()->json([
            'success' => true,
            'report' => $report[1],
            'audit' => $audit,
        ], Response::HTTP_OK);
    }

    private function buildReport($auditResults, $organization) {
        // If the audit results are empty, return an error response
        if (empty($auditResults)) {
            return response()->json([
                'success' => false,
                'message' => 'No audit results found.',
            ], Response::HTTP_NOT_FOUND);
        }

        // Get weak accounts and their names
        $weakAccounts = array_filter($auditResults, function($result) {
            return !$result['success'];
        });

        // If no weak accounts found, return a success message
        if (empty($weakAccounts)) {
            return [true, "Audit succeeded. No weak passwords found."];
        }

        $weakAccountNames = implode(', ', array_map(function($result) {
            return $result['name'];
        }, $weakAccounts));

        // Find the maximum allowed weak passwords for the organization
        $maxWeakPasswords = $organization->max_weak_passwords;
        // Get the total number of weak passwords
        $weakCount = count($weakAccounts);
        // Return the audit report with the results
        if ($weakCount > $maxWeakPasswords) {
            return [false, "Audit failed. Weak passwords found for: {$weakAccountNames}"];
        } else {
            return [true, "Audit succeeded. Weak passwords found for: {$weakAccountNames}"];
        }
    }

    private function calculatePasswordStrength($password) {
        $length = strlen($password); // Password length
        $uppercaseCount = $this->countUppercase($password); // Count of uppercase letters
        $numbersCount = $this->countNumbers($password); // Count of numeric characters
        $punctuationCount = $this->countPunctuation($password); // Count of punctuation characters
        $strength = $length + ($uppercaseCount * 2) + ($numbersCount * 2) + ($punctuationCount * 3); // Calculate total password strength

        return $strength; 
    }

    private function countUppercase($s)
    {
        $cnt = 0;
        foreach (str_split($s) as $c) {
            if (ctype_upper($c)) {
                $cnt++;
            }
        }
        return $cnt;
    }
    private function countNumbers($s)
    {
        $cnt = 0;
        foreach (str_split($s) as $c) {
            if (ctype_digit($c)) {
                $cnt++;
            }
        }
        return $cnt;
    }
    private function countPunctuation($s)
    {
        $cnt = 0;
        foreach (str_split($s) as $c) {
            if (ctype_punct($c)) {
                $cnt++;
            }
        }
        return $cnt;
    }
}
