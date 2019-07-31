pipeline {
    agent any

    stages {
        stage('Build') {
            steps {
                sh 'COMMIT=${GIT_COMMIT} docker-compose -f ./.scope/docker-compose.yml -p ${GIT_COMMIT} build'
            }
        }
        stage('Test') {
            steps {
                sh 'COMMIT=${GIT_COMMIT} docker-compose -f ./.scope/docker-compose.yml -p ${GIT_COMMIT} up --exit-code-from=scope-test scope-test'
            }
        }
    }
    post {
        always {
           sh 'COMMIT=${GIT_COMMIT} docker-compose -f ./.scope/docker-compose.yml -p ${GIT_COMMIT} down -v'
        }
    }
}