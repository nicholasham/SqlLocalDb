'use strict';
var Nuget = require('nuget-runner');

var nuspecProperties = {
    authors: 'Nicholas Hammond',
    owners: 'Nicholas Hammond',
    licenseUrl: 'http://github.com',
    projectUrl: 'http://github.com',
    iconUrl: 'http://github.com',
    requireLicenseAcceptance: false,
    releaseNotes: '',
    copyright: 'Copyright (2015)',
    configuration: buildinfo.configuration
};


gulp.task('nuget-clean', function () {

            del('/src/Packages', function (err, paths) {
                console.log('Deleted files/folders:\n', paths.join('\n'));
                done();
            });
        }

        gulp.task('nuget-pack', ['nuget-restore', 'build', 'test'], function () {


            fs.mkdirSync(paths.artifactsDirectory);

            var options = {
                nonull: false
            };

            glob('**/*.nuspec', options, function (error, nuspecFiles) {


                if (nuspecFiles.length === 0) {
                    util.log('NuGet: No nuspec files found in projects to pack');
                    return;
                }

                for (var index = 0; index < nuspecFiles.length; index++) {

                    var nuspecFile = nuspecFiles[index];
                    var projectFile = nuspecFile.replace('.nuspec', '.csproj');

                    nuget.pack({
                        spec: projectFile,
                        outputDirectory: paths.artifactsDirectory,
                        version: buildinfo.version,
                        symbols: true,
                        includeReferencedProjects: true,
                        properties: nuspecProperties
                    });
                }
            })


        });