import React, { Fragment } from 'react'
import moment from 'moment'

import VpnKeyIcon from '@material-ui/icons/VpnKey'
import AddIcon from '@material-ui/icons/Add'
import DeleteIcon from '@material-ui/icons/Delete'
import EditIcon from '@material-ui/icons/Edit'
import { Skeleton } from '@material-ui/lab'
import MuiAlert from '@material-ui/lab/Alert'

import { Paper, Typography, Grid, Chip, Button, LinearProgress, TextField, Snackbar } from '@material-ui/core'

import { ApiKeyEntity, DomainEntity } from '../../../entities/api'
import { ApiKeysController } from './ApiKeysController'
import ButtonWithLoading from '../../../components/ButtonWithLoading'
import LoadingError from '../../../components/LoadingError'
import AddModal from '../../../components/AddModal'

import './ApiKeys.less'
import ConfirmationModal from '../../../components/ConfirmationModal'
import Snack from '../../../components/Snack'

export interface ApiKeysProps {}
export interface ApiKeysState {
    isLoaded: boolean
    apiKeys: ApiKeyEntity[]
    apiKeysLoading: boolean
    apiKeysError: boolean
    apiKeyLoading: boolean
    apiKeyError: boolean
    modalAddDomain: {
        visible: boolean
        keyEntity?: ApiKeyEntity
        domainName: string
        error: boolean
    }
    modalDeleteDomain: {
        visible: boolean
        keyEntity?: ApiKeyEntity
        domainEntity?: DomainEntity
    }
    modalDeleteApiKey: {
        visible: boolean
        keyEntity?: ApiKeyEntity
    }
    modalUpdateApiKey: {
        visible: boolean
        keyEntity?: ApiKeyEntity
    }
    successMessage: {
        message: string
        visible: boolean
    }
    errorMessage: {
        message: string
        visible: boolean
    }
}

class ApiKeys extends React.Component<ApiKeysProps, ApiKeysState> {
    private controller?: ApiKeysController = undefined

    constructor(props: ApiKeysProps) {
        super(props)
        this.controller = new ApiKeysController(this)
        this.state = {
            isLoaded: false,
            apiKeys: [],
            apiKeysLoading: false,
            apiKeysError: false,
            apiKeyLoading: false,
            apiKeyError: false,
            modalAddDomain: {
                visible: false,
                domainName: '',
                error: false,
            },
            modalDeleteDomain: {
                visible: false,
            },
            modalDeleteApiKey: {
                visible: false,
            },
            modalUpdateApiKey: {
                visible: false,
            },
            successMessage: {
                message: '',
                visible: false,
            },
            errorMessage: {
                message: '',
                visible: false,
            },
        }
    }

    componentDidMount() {
        this.controller!.initialize()
    }

    render() {
        return (
            <Fragment>
                {!this.state.isLoaded && <LinearProgress />}
                <Typography variant="h2" className="override-h2" component="h2">
                    <VpnKeyIcon /> Api keys
                </Typography>
                <br />
                {this.state.apiKeysError ? (
                    <React.Fragment>
                        <LoadingError />
                    </React.Fragment>
                ) : (
                    <React.Fragment>
                        {this.state.apiKeys.length < 10 ? (
                            <ButtonWithLoading loading={this.state.apiKeyLoading} variant="contained" color="primary" onClick={this.controller!.createApiKey}>
                                Generate new key
                            </ButtonWithLoading>
                        ) : (
                            <React.Fragment>
                                <p>You have reached the maximum api keys. If you want to create a new one, please delete one of your current api keys before doing so.</p>
                                <br />
                            </React.Fragment>
                        )}

                        {this.state.apiKeysLoading ? (
                            <React.Fragment>
                                <Skeleton height={200} className="paper paper-no-padding" />
                                <Skeleton height={200} className="paper paper-no-padding" />
                            </React.Fragment>
                        ) : (
                            <React.Fragment>
                                {this.state.apiKeys &&
                                    this.state.apiKeys.map(key => (
                                        <Paper className="paper paper-no-padding" key={key.apiKeyId!}>
                                            <Grid container spacing={1}>
                                                <Grid item xs={12} md={2} className="api-key-card-left grid-paper-padding">
                                                    <Grid container direction="column" justify="space-between" alignItems="flex-start" style={{ height: '100%' }}>
                                                        <Grid item>
                                                            {moment(key.expiresOn!) < moment() ? (
                                                                <Chip size="small" label="Expired" className="chip-red" />
                                                            ) : key.domains!.length > 0 ? (
                                                                <Chip size="small" label="Active" className="chip-green" />
                                                            ) : (
                                                                <Chip size="small" label="Inactive" />
                                                            )}
                                                        </Grid>
                                                        <Grid item>
                                                            <Typography variant="caption" className="override-caption" component="span" style={{ color: '#ddd' }}>
                                                                Expires on{' '}
                                                                <span style={{ whiteSpace: 'nowrap' }}>
                                                                    {moment(key.expiresOn!).format('YYYY-MM-DD')} <EditIcon onClick={() => {}} cursor="pointer" />
                                                                </span>
                                                            </Typography>
                                                        </Grid>
                                                    </Grid>
                                                </Grid>
                                                <Grid item xs={12} md={10} className="grid-paper-padding">
                                                    <Grid container direction="column" justify="space-between" alignItems="flex-start" style={{ height: '100%' }}>
                                                        <Grid item style={{ height: '35px', width: '100%' }}>
                                                            <Grid container justify="space-between" alignItems="flex-start" style={{ width: '100%', display: 'flex' }}>
                                                                <Grid item style={{ verticalAlign: 'middle' }}>
                                                                    <Typography variant="h3" className="override-h2" component="h3">
                                                                        <VpnKeyIcon />
                                                                        {key.key}
                                                                    </Typography>
                                                                </Grid>
                                                                <Grid item style={{ alignSelf: 'flex-start', paddingRight: '5px' }}>
                                                                    <Button color="secondary" size="small" onClick={() => this.controller!.showDeleteApiKey(key)}>
                                                                        <DeleteIcon />
                                                                    </Button>
                                                                </Grid>
                                                            </Grid>
                                                        </Grid>
                                                        <Grid item>
                                                            {key.domains!.length > 0 ? (
                                                                key.domains!.map(d => (
                                                                    <Chip
                                                                        key={d.domainId!}
                                                                        size="small"
                                                                        variant="default"
                                                                        color="primary"
                                                                        label={d.name!}
                                                                        className="chip-spaced"
                                                                        deleteIcon={<DeleteIcon />}
                                                                        onDelete={() => this.controller!.showDeleteDomain(key, d)}
                                                                    />
                                                                ))
                                                            ) : (
                                                                <span style={{ color: '#f44336' }}>Please add a domain to activate the api key.</span>
                                                            )}
                                                        </Grid>
                                                        <Grid item style={{ height: '45px', display: 'flex' }}>
                                                            {moment(key.expiresOn!) > moment() && (
                                                                <Button color="primary" size="small" variant="outlined" style={{ alignSelf: 'flex-end' }} onClick={() => this.controller!.showAddDomain(key)}>
                                                                    <AddIcon /> Add domain
                                                                </Button>
                                                            )}
                                                        </Grid>
                                                    </Grid>
                                                </Grid>
                                            </Grid>
                                        </Paper>
                                    ))}
                            </React.Fragment>
                        )}
                    </React.Fragment>
                )}

                <AddModal
                    title="Add domain to api key"
                    open={this.state.modalAddDomain.visible}
                    onCancel={this.controller!.hideAddDomain}
                    onConfirm={() => {
                        this.controller!.createDomain(this.state.modalAddDomain.keyEntity!.apiKeyId!, this.state.modalAddDomain.domainName)
                        this.controller!.hideAddDomain()
                    }}
                    confirmActive={!this.state.modalAddDomain.error && this.state.modalAddDomain.domainName.length > 0}
                >
                    <React.Fragment>
                        <MuiAlert severity="info">{this.state.modalAddDomain.keyEntity && this.state.modalAddDomain.keyEntity!.key}</MuiAlert>
                        <p>Enter the domain name without (ex: domain.com) you want to add to this api key.</p>
                        <p>Please note that by default "localhost" is already working and that the "www" version of your domain will also work.</p>
                        <TextField
                            onChange={this.controller!.handleAddDomainTyping}
                            name="domain"
                            type="text"
                            label="Domain name"
                            fullWidth
                            value={this.state.modalAddDomain.domainName}
                            error={this.state.modalAddDomain.error}
                            helperText={this.state.modalAddDomain.error && "Domain name is invalid. Please enter something like 'domain.com'."}
                        />
                    </React.Fragment>
                </AddModal>

                <ConfirmationModal
                    title="Delete api key"
                    open={this.state.modalDeleteApiKey.visible}
                    onCancel={this.controller!.hideDeleteApiKey}
                    onConfirm={() => {
                        this.controller!.deleteApiKey(this.state.modalDeleteApiKey.keyEntity!.apiKeyId!)
                        this.controller!.hideDeleteApiKey()
                    }}
                >
                    <React.Fragment>
                        <MuiAlert severity="info">
                            Api key: <b>{this.state.modalDeleteApiKey.keyEntity && this.state.modalDeleteApiKey.keyEntity!.key}</b>
                        </MuiAlert>
                        <p>Are you sure you want to delete this api key ?</p>
                        <p style={{ color: 'red' }}>All associated editors will refuse to clean pasted data immediately.</p>
                    </React.Fragment>
                </ConfirmationModal>

                <ConfirmationModal
                    title="Delete domain"
                    open={this.state.modalDeleteDomain.visible}
                    onCancel={this.controller!.hideDeleteDomain}
                    onConfirm={() => {
                        this.controller!.deleteDomain(this.state.modalDeleteDomain.keyEntity!.apiKeyId!, this.state.modalDeleteDomain.domainEntity!.domainId!)
                        this.controller!.hideDeleteDomain()
                    }}
                >
                    <React.Fragment>
                        <MuiAlert severity="info">
                            Api key: <b>{this.state.modalDeleteDomain.keyEntity && this.state.modalDeleteDomain.keyEntity!.key}</b>
                            <br />
                            Domain: <b>{this.state.modalDeleteDomain.domainEntity && this.state.modalDeleteDomain.domainEntity!.name}</b>
                        </MuiAlert>
                        <p>Are you sure you want to delete this domain ?</p>
                        <p style={{ color: 'red' }}>All associated editors will refuse to clean pasted data immediately.</p>
                    </React.Fragment>
                </ConfirmationModal>

                <Snack type="success" visible={this.state.successMessage.visible} onClose={this.controller!.hideSuccessSnackbar}>
                    {this.state.successMessage.message}
                </Snack>

                <Snack type="error" visible={this.state.errorMessage.visible} onClose={this.controller!.hideErrorSnackbar}>
                    {this.state.errorMessage.message}
                </Snack>
            </Fragment>
        )
    }
}

export default ApiKeys
